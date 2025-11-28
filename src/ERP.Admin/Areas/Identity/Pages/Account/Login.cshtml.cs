using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        // 1. CAMPOS PRIVADOS (Aquí se guardan las herramientas inyectadas)
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        // 2. CONSTRUCTOR (Aquí se inyectan las dependencias)
        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // 3. PROPIEDAD INPUT (Aquí se guardan los datos del formulario)
        [BindProperty]
        public InputModel Input { get; set; }

        // Esta propiedad guarda la URL a donde volver después de loguearse
        public string ReturnUrl { get; set; }

        // Mensaje de error (opcional)
        [TempData]
        public string ErrorMessage { get; set; }

        // 4. CLASE INPUTMODEL (Define qué datos esperamos del usuario)
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

        // 5. MÉTODO GET (Carga la página)
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Limpiar cookies externas para asegurar un login limpio
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        // 6. MÉTODO POST (Procesa el login)
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Si returnUrl es nulo, lo mandamos a la raíz (que redirige al Dashboard)
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Intentamos loguear al usuario
                // lockoutOnFailure: false (para que no bloquee la cuenta si falla)
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    // AQUÍ ESTÁ EL MENSAJE DE ERROR PERSONALIZADO
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                    return Page();
                }
            }

            // Si llegamos aquí, algo falló en la validación del modelo (ej. campos vacíos)
            return Page();
        }
    }
}