using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Trabalho.Areas.Data;
using Trabalho.Models;

public class LoginModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly Lab2Context _context;

    public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger, UserManager<AppUser> userManager, Lab2Context context)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {

            // Verificar se o user está bloqueado
            var user = await _userManager.FindByNameAsync(Input.Username); // Obter o user
            var bloqueio = _context.Bloquears
                .Where(b => b.Username == Input.Username && b.Bloqueado == true)
                .OrderByDescending(b => b.DataBloqueio)
                .FirstOrDefault();

            if (bloqueio != null)
            {
                var motivoBloqueio = _context.MotivoBloqs
                    .FirstOrDefault(mb => mb.IdMotivoBloq == bloqueio.IdMotivoBloq);

                if (motivoBloqueio != null)
                {
                    ModelState.AddModelError(string.Empty, $"A sua conta está bloqueada! Motivo: {motivoBloqueio.Descricao}");
                }
                return Page(); // Retorna a página de login com a mensagem de erro
            }



            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                // Verifique se o utilizador é Leitor, Bibliotecário ou Administrador
                var bibliotecario = _context.Bibliotecarios.FirstOrDefault(b => b.UsernameBib == user.UserName);
                var leitor = _context.Leitores.FirstOrDefault(l => l.UsernameLei == user.UserName);
                var admin = _context.Administradores.FirstOrDefault(a => a.UsernameAdmin == user.UserName);
                var validarRegisto = _context.ValidarRegistos.FirstOrDefault(vr => vr.UsernameBib == user.UserName);

                // Atribuir a role 'Leitor' ao utilizador
                if (leitor != null && !await _userManager.IsInRoleAsync(user, "Leitor"))
                {
                    await _userManager.AddToRoleAsync(user, "Leitor");
                    _logger.LogInformation($"Role 'Leitor' atribuída ao utilizador {user.UserName}");
                }

                // Atribuir a role 'Administrador' ao utilizador
                if (admin != null && !await _userManager.IsInRoleAsync(user, "Administrador"))
                {
                    await _userManager.AddToRoleAsync(user, "Administrador");
                    _logger.LogInformation($"Role 'Administrador' atribuída ao utilizador {user.UserName}");
                }

                // Atribuir a role 'Bibliotecário' ao utilizador
                if (bibliotecario != null && validarRegisto != null && validarRegisto.Validado && !await _userManager.IsInRoleAsync(user, "Bibliotecario"))
                {
                    await _userManager.AddToRoleAsync(user, "Bibliotecario");
                    _logger.LogInformation($"Role 'Bibliotecario' atribuída ao utilizador {user.UserName}");
                }
                else if (bibliotecario != null && (validarRegisto == null || !validarRegisto.Validado))
                {
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, "O seu registro está pendente... Aguarde pela aprovação.");
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user); // Isso irá atualizar as claims de identidade

                if (await _userManager.IsInRoleAsync(user, "Administrador"))
                {
                    return RedirectToAction("Index", "Administradores");
                }

                if (await _userManager.IsInRoleAsync(user, "Bibliotecario"))
                {
                    return RedirectToAction("Index", "Livro");
                }

                if (await _userManager.IsInRoleAsync(user, "Leitor"))
                {
                    return RedirectToAction("Biblioteca", "Leitores");
                }

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
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        return Page();
    }
}