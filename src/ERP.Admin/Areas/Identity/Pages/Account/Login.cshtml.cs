using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        // 1. PRIVATE FIELDS (Injected services are stored here)
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        // 2. CONSTRUCTOR (Dependencies are injected here)
        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // 3. INPUT PROPERTY (Holds form data)
        [BindProperty]
        public InputModel Input { get; set; }

        // This property holds the URL to return to after login
        public string ReturnUrl { get; set; }

        // Error message (optional)
        [TempData]
        public string ErrorMessage { get; set; }

        // 4. INPUTMODEL CLASS (Defines expected user inputs)
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        // 5. GET METHOD (Loads the page)
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear external cookies to ensure a clean login
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        // 6. POST METHOD (Processes the login)
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // If returnUrl is null, redirect to the root (which redirects to Dashboard)
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Attempt to sign in the user
                // lockoutOnFailure: false (do not lock account on failures)
                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage(
                        "./LoginWith2fa",
                        new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe }
                    );
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // CUSTOM ERROR MESSAGE
                    ModelState.AddModelError(string.Empty, "Incorrect username or password.");
                    return Page();
                }
            }

            // If we reach here, model validation failed (e.g., empty fields)
            return Page();
        }
    }
}
