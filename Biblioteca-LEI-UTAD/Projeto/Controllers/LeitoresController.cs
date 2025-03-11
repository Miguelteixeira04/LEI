using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho.Models;

namespace Trabalho.Controllers
{
    [Authorize(Roles = "Leitor")]
    public class LeitoresController : Controller
    {
        private readonly Lab2Context _context;
        private readonly ILogger<LeitoresController> _logger;

        public LeitoresController(Lab2Context context, ILogger<LeitoresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Informacoes()
        {
            var biblioteca = await _context.Bibliotecas.ToListAsync();
            return View(biblioteca);
        }

        public async Task<IActionResult> Biblioteca(string titulo, string autor, string genero)
        {
            var query = _context.Livros
                .Include(l => l.IdAutorNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
            {
                query = query.Where(l => EF.Functions.Like(l.Titulo, $"%{titulo}%"));
            }

            if (!string.IsNullOrEmpty(autor))
            {
                query = query.Where(l => EF.Functions.Like(l.IdAutorNavigation.NameAutor, $"%{autor}%"));
            }

            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(l => EF.Functions.Like(l.Genero, $"%{genero}%"));
            }

            var livros = await query.ToListAsync();

            ViewData["Titulo"] = titulo;
            ViewData["Autor"] = autor;
            ViewData["Genero"] = genero;

            return View(livros);
        }

        public async Task<IActionResult> Requisitar([FromBody] Requisicao model)
        {
            var livro = await _context.Livros.FirstOrDefaultAsync(l => l.Isbn == model.Isbn);
            if (livro == null || livro.NExemplares <= 0)
            {
                return Json(new { sucesso = false, mensagem = "Não há exemplares disponíveis para este livro." });
            }

            var usernameLei = User.Identity.Name;

            // Gera um ID único para a requisição
            int nextIdRequisicao;
            do
            {
                nextIdRequisicao = new Random().Next(10000, 99999); // Gera um ID de 5 dígitos
            } while (await _context.Requisicaos.AnyAsync(a => a.IdRequisicao == nextIdRequisicao)); // Verifica unicidade no banco

            // Adicionar requisição à tabela Requisicao
            var requisicao = new Requisicao
            {
                IdRequisicao = nextIdRequisicao,
                UsernameBib = "ruano_04",
                UsernameLei = usernameLei,
                Isbn = model.Isbn,
                DataRequisicao = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(15),
                Estado = true  // Requisitado
            };

            _context.Requisicaos.Add(requisicao);

            // Atualizar o número de exemplares no livro
            livro.NExemplares -= 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar alterações no banco de dados.");
                return Json(new { sucesso = false, mensagem = "Erro ao salvar alterações no banco de dados: " + ex.Message });
            }

            // Retornar uma resposta JSON com sucesso
            return Json(new { sucesso = true });
        }
        public async Task<IActionResult> History()
        {
            var usernameLei = User.Identity.Name;

            var historico = await _context.Requisicaos
                .Where(r => r.UsernameLei == usernameLei)
                .Include(r => r.IsbnNavigation)
                .ToListAsync();

            return View(historico);
        }
    }
}
