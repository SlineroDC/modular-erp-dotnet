using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP.Api.Dto;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return BadRequest(new { message = "The email is already registered." });

        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = "Error creating user", errors = result.Errors });
        }

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

        return Ok(new { message = "Successfully registered user." });
    }
}
