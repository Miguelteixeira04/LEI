using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Trabalho.Areas.Data;
using Trabalho.Models;


namespace Trabalho.Controllers
{
    public class AdministradoresController : Controller
    {
        private readonly Lab2Context _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IEmailSender _emailSender;

        public AdministradoresController(Lab2Context context, UserManager<AppUser> userManager, IUserStore<AppUser> userStore, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        // GET: Administradores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administradores.ToListAsync());
        }

        public IActionResult RegistarAdmin()
        {
            return View();
        }

        public IActionResult AprovarNovosBiblio()
        {
            return View();
        }

        public IActionResult GerirUtilizadores()
        {
            return View();
        }

        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("UsernameAdmin,Nome,Email,Password")] Administradore admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        //[HttpGet]
        public IActionResult GerirPerfil(int userId)  // Recebe o userId através da query string
        {
            return View();
        }


        public IActionResult MotivoBlock()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Administradores/GetBibliotecariosParaAprovar")]
        public async Task<IActionResult> GetBibliotecariosParaAprovar()
        {
            try
            {
                // Filtra diretamente os usuários cujo Role é "Bibliotecario" e que não foram validados
                var bibliotecarios = await _context.Users
                    .Where(u => u.Role == "Bibliotecario" &&
                                !_context.ValidarRegistos.Any(vr => vr.UsernameBib == u.UserName && vr.Validado == true))
                    .Join(_context.Autenticados,
                        u => u.UserName, // Faz a junção pelo Username
                        a => a.Username,
                        (u, a) => new
                        {
                            Id = u.Id,
                            Nome = a.Nome, // Obtém o nome da tabela Autenticado
                            Username = u.UserName,
                            Role = u.Role
                        })
                    .ToListAsync();

                // Verifique se os dados de "Nome" estão sendo recuperados corretamente
                foreach (var bibliotecario in bibliotecarios)
                {
                    Console.WriteLine($"Nome: {bibliotecario.Nome} | Username: {bibliotecario.Username}");

                }

                // Verifica se há bibliotecários na lista
                if (bibliotecarios == null || bibliotecarios.Count == 0)
                {
                    return Ok(new List<object>());
                }

                return Ok(bibliotecarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter bibliotecários: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/Administradores/Criar")]
        public async Task<IActionResult> RegistarAdmin([FromBody] Administradore administrador)
        {
            // Validação básica dos dados recebidos
            if (administrador == null ||
                string.IsNullOrWhiteSpace(administrador.UsernameAdmin) ||
                string.IsNullOrWhiteSpace(administrador.Nome) ||
                string.IsNullOrWhiteSpace(administrador.Email) ||
                string.IsNullOrWhiteSpace(administrador.Password))
            {
                return BadRequest("Todos os campos são obrigatórios.");
            }

            try
            {
                // Verifica se o email já está registrado
                var emailExistente = await _context.Users.AnyAsync(u => u.Email == administrador.Email);
                if (emailExistente)
                {
                    return BadRequest("Já existe um usuário com este email.");
                }

                // Verifica se o username já está registrado
                var usernameExistente = await _context.Users.AnyAsync(u => u.UserName == administrador.UsernameAdmin);
                if (usernameExistente)
                {
                    return BadRequest("Já existe um usuário com este username.");
                }

                // Cria um novo objeto de usuário para salvar na tabela Users
                var novoUsuario = new AppUser
                {
                    Nome = administrador.Nome,
                    Email = administrador.Email,
                    UserName = administrador.UsernameAdmin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(administrador.Password), // Senha encriptada
                    //Role = "Administrador"
                };

                // Salvar no banco de dados
                _context.Users.Add(novoUsuario);
                await _context.SaveChangesAsync();

                // Retornar resposta de sucesso
                return Ok(new { message = "Administrador criado com sucesso." });
            }
            catch (Exception ex)
            {
                // Em caso de erro, retornar uma resposta adequada
                return StatusCode(500, $"Erro ao criar administrador: {ex.Message}");
            }
        }


        // Obter a lista de todos os utilizadores
        [HttpGet]
        [Route("api/Administradores/Utilizadores")]
        public async Task<IActionResult> ObterUtilizadores()
        {
            try
            {
                // Retorna os utilizadores da base de dados
                var utilizadores = await _context.Users
                    .Select(u => new
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Username = u.UserName,
                        Role = u.Role // Depois vêr na entidade (O que está dentro da pasta models e depois na entidade_que_queres.cs) se são só estes atributos que precisas
                    })
                    .ToListAsync();


                if (utilizadores == null || utilizadores.Count == 0)
                {
                    return NotFound("Nenhum utilizador encontrado.");
                }

                return Ok(utilizadores);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter utilizadores: {ex.Message}");
            }

        }

        // Obter apenas UM utilizador
        // Endpoint para buscar o utilizador por ID
        [HttpGet]
        [Route("api/Administradores/Utilizador/{username}")]
        public async Task<IActionResult> GetUtilizador(string username)
        {
            try
            {
                var utilizador = await _context.Users
                    .Where(u => u.UserName == username)
                    .Select(u => new
                    {
                        u.UserName,
                        u.Email,
                        u.Nome,
                        u.Address,
                        u.PhoneNumber,
                        u.Role,
                        Estado = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.Now ? "Bloqueado" : "Ativo"
                    })
                    .FirstOrDefaultAsync();

                if (utilizador == null)
                {
                    return NotFound(new { mensagem = "Utilizador não encontrado." });
                }

                return Ok(utilizador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao carregar utilizador: {ex.Message}" });
            }
        }


        // Bloquear Utilizadores
        [HttpPost]
        [Route("api/Administradores/BloquearUtilizador/{username}")]
        public async Task<IActionResult> BloquearUtilizador(string username, string motivo)
        {
            try
            {
                motivo = "Vários atrasos na devolução de livros";
                var utilizador = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (utilizador == null)
                {
                    return NotFound(new { mensagem = "Utilizador não encontrado." });
                }

                // Gerar novo ID para MotivoBloq
                int novoIdMotivoBloq = (await _context.MotivoBloqs
                    .OrderByDescending(m => m.IdMotivoBloq)
                    .Select(m => (int?)m.IdMotivoBloq)
                    .FirstOrDefaultAsync() ?? 9999) + 1;

                if (novoIdMotivoBloq < 10000 || novoIdMotivoBloq > 999999)
                {
                    return StatusCode(500, new { mensagem = "Erro ao gerar o ID para o motivo do bloqueio. Intervalo excedido." });
                }

                var motivoBloq = new MotivoBloq
                {
                    IdMotivoBloq = novoIdMotivoBloq,
                    Descricao = motivo
                };

                _context.MotivoBloqs.Add(motivoBloq);

                // Gerar novo ID para Bloquear
                int novoIdBloqueio = (await _context.Bloquears
                    .OrderByDescending(b => b.IdBloqueio)
                    .Select(b => (int?)b.IdBloqueio)
                    .FirstOrDefaultAsync() ?? 9999) + 1;

                if (novoIdBloqueio < 10000 || novoIdBloqueio > 999999)
                {
                    return StatusCode(500, new { mensagem = "Erro ao gerar o ID para o bloqueio. Intervalo excedido." });
                }

                var bloqueio = new Bloquear
                {
                    IdBloqueio = novoIdBloqueio,
                    IdMotivoBloq = motivoBloq.IdMotivoBloq,
                    UsernameAdmin = "Admin", // User.Identity?.Name ?? "AdminDesconhecido",
                    Username = utilizador.UserName,
                    DataBloqueio = DateTime.Now,
                    Bloqueado = true
                };

                _context.Bloquears.Add(bloqueio);

                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    user.LockoutEnd = DateTimeOffset.Now.AddYears(1);
                    await _userManager.UpdateAsync(user);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Utilizador bloqueado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = $"Erro ao bloquear utilizador: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("api/Administradores/AprovarBibliotecarios")]
        public async Task<IActionResult> AprovarBibliotecarios([FromBody] List<string> usernames)
        {
            try
            {
                if (usernames == null || usernames.Count == 0)
                {
                    return BadRequest("Nenhum bibliotecário foi enviado para aprovação.");
                }

                // Atualizar os registros na tabela ValidarRegistos
                foreach (var username in usernames)
                {
                    var registro = await _context.ValidarRegistos
                        .FirstOrDefaultAsync(vr => vr.UsernameBib == username);

                    if (registro != null)
                    {
                        registro.Validado = true;
                        _context.ValidarRegistos.Update(registro);
                    }
                    else
                    {
                        // Adiciona um novo registro caso não exista
                        _context.ValidarRegistos.Add(new ValidarRegisto
                        {
                            UsernameBib = username,
                            Validado = true
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("Bibliotecários aprovados com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao aprovar bibliotecários: {ex.Message}");
            }
        }

        private bool AdministradoresExists(string id)
        {
            return _context.Administradores.Any(e => e.UsernameAdmin == id);
        }
    }
}
