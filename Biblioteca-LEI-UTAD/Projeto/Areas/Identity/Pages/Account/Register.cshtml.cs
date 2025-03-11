// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Trabalho.Areas.Data;
using Trabalho.Models;

namespace Trabalho.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly Lab2Context _context;


        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            Lab2Context context)

        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;

        }


        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O nome é obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "O username é obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }


            [Required(ErrorMessage = "O tipo de utilizador é obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Role")]
            public string Role { get; set; }

            public string Address { get; set; }

            [Required(ErrorMessage = "O nº de telefone é obrigatório.")]
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Phone]
            [Display(Name = "PhoneNumber2")]
            public string PhoneNumber2 { get; set; }

            [Phone]
            [Display(Name = "PhoneNumber3")]
            public string PhoneNumber3 { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
            [MinLength(6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Verificar se o Email já existe
                if (Input.Role != "Bibliotecario" && !string.IsNullOrEmpty(Input.Email))
                {
                    var existingUserByEmail = await _userManager.FindByEmailAsync(Input.Email);
                    if (existingUserByEmail != null)
                    {
                        ModelState.AddModelError(string.Empty, "O email já está em uso.");
                        return Page();
                    }
                }

                var user = new AppUser
                {
                    Nome = Input.Nome,
                    Role = Input.Role,
                    PhoneNumber = Input.PhoneNumber,
                    PhoneNumber2 = Input.PhoneNumber2,
                    PhoneNumber3 = Input.PhoneNumber3,
                    UserName = Input.Username,
                };

                // Se o user for bibliotecário, o endereço e o email devem ser nulos
                if (Input.Role == "Bibliotecario")
                {
                    ModelState.Remove("Input.Email"); // Remove validação do email
                    ModelState.Remove("Input.Address"); // Remove validação do endereço
                    user.EmailConfirmed = true; // Confirmação automática do email
                }
                else
                {
                    user.Address = Input.Address;  // Para leitores, o endereço será preenchido
                    user.Email = Input.Email;
                }

                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                }

                user.PhoneNumberConfirmed = true;

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Inserir dados na tabela Autenticado
                    var autenticado = new Autenticado
                    {
                        Username = user.UserName,
                        Password = Input.Password,
                        Contacto = user.PhoneNumber,
                        Nome = user.Nome,
                        Contacto2 = user.PhoneNumber2,
                        Contacto3 = user.PhoneNumber3
                    };

                    _context.Autenticados.Add(autenticado); // Adiciona o novo registro
                    await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

                    // Inserir dados na tabela específica com base na Role
                    if (Input.Role == "Bibliotecario")
                    {
                        var bibliotecario = new Bibliotecario
                        {
                            UsernameBib = user.UserName
                        };

                        _context.Bibliotecarios.Add(bibliotecario);

                        // Gera um ID crescente começa em 10001
                        var idValid = 10000 + _context.Bibliotecarios.Count(); // Começa em 10001

                        // Verifica se o ID gerado ultrapassa 99999
                        if (idValid > 99999)
                        {
                            ModelState.AddModelError(string.Empty, "Não é possível gerar mais IDs para Bibliotecários. Limite atingido.");
                            return Page(); // Retorna se o limite de IDs for alcançado
                        }

                        var validarRegisto = new ValidarRegisto
                        {
                            IdValid = idValid,
                            UsernameBib = user.UserName,
                            Validado = false,
                            UsernameAdmin = "Admin",  // Deixe vazio
                            DataValid = new DateTime(1901, 1, 1)  // Use a data válida mínima
                        };

                        _context.ValidarRegistos.Add(validarRegisto);
                    }
                    else if (Input.Role == "Leitor")
                    {
                        var leitor = new Leitore
                        {
                            UsernameLei = user.UserName,
                            Morada = Input.Address,
                            Email = Input.Email,
                        };

                        _context.Leitores.Add(leitor);
                    }

                    await _context.SaveChangesAsync(); // Salva as alterações na tabela específica

                    // Enviar e-mail de confirmação
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email",
                            $"Por favor confirme a sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> aqui </a>.");
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error registering user: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se chegamos até aqui, algo falhou, mostrar novamente o formulário
            return Page();
        }


        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }


    }
}
