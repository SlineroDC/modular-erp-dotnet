using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP.Api.Dto;
using ERP.Api.DTo;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager,
        ICustomerRepository customerRepository,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _customerRepository = customerRepository;
        _configuration = configuration;
    }

    // ==========================================
    // 1. LOGIN
    // ==========================================
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            // Get roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Create claims (information inside the token)
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!), // Email is usually the username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate security key
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            // Create the token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            return Ok(
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = new { user.Email, user.UserName },
                }
            );
        }

        return Unauthorized(new { message = "Incorrect username or password." });
    }

    // ==========================================
    // 2. REGISTER
    // ==========================================
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        // A. Validate if user already exists in Identity
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return BadRequest(new { message = "The email is already registered." });

        // B. Validate if document/email already exists in business (duplicate)
        var isDuplicate = await _customerRepository.IsDuplicateAsync(model.Email, model.IdDocument);
        if (isDuplicate)
            return BadRequest(new { message = "Document or email already belong to a customer." });

        // C. Crear Usuario de Identity (Login)
        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            // Return the first error to show in the frontend
            return BadRequest(new { message = result.Errors.First().Description });
        }

        // D. Crear Cliente de Negocio
        var customer = new Customer
        {
            Name = model.Name,
            LastName = model.LastName,
            Email = model.Email,
            IdDocument = model.IdDocument,
            Phone = model.Phone,
            Address = model.Address,
            IsActive = true,
        };

        await _customerRepository.AddAsync(customer);

        return Ok(new { message = "User registered successfully." });
    }

    // ==========================================
    // 3. PROFILE - GET
    // ==========================================
    [HttpGet("profile")]
    [Authorize] // Requiere Token
    public async Task<IActionResult> GetProfile()
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound("User not found.");

        // Retrieve customer data
        var customersPage = await _customerRepository.GetAllAsync(1, 1, email);
        var customer = customersPage.Items.FirstOrDefault();

        var profile = new UserProfileDto
        {
            UserName = user.UserName!,
            Email = user.Email!,
            Name = customer?.Name ?? "",
            LastName = customer?.LastName ?? "",
            Document = customer?.IdDocument ?? "",
            Phone = customer?.Phone ?? "",
            Address = customer?.Address ?? "",
        };

        return Ok(profile);
    }

    // ==========================================
    // 4. PROFILE - UPDATE (PUT)
    // ==========================================
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByEmailAsync(email!);
        if (user == null)
            return Unauthorized();

        // A. Change password
        if (
            !string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword)
        )
        {
            var passResult = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );
            if (!passResult.Succeeded)
                return BadRequest(new { message = passResult.Errors.First().Description });
        }

        // B. Update customer data
        var customersPage = await _customerRepository.GetAllAsync(1, 1, email);
        var customer = customersPage.Items.FirstOrDefault();

        if (customer != null)
        {
            customer.Name = model.Name;
            customer.LastName = model.LastName;
            customer.Phone = model.Phone;
            customer.Address = model.Address;

            await _customerRepository.UpdateAsync(customer);
        }

        return Ok(new { message = "Profile updated successfully." });
    }
}
