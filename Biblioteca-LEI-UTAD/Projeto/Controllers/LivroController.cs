using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Trabalho.Models;

namespace Trabalho.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class LivroController : Controller
    {
        private readonly Lab2Context _context;

        public LivroController(Lab2Context context)
        {
            _context = context;
        }

        // GET: Livro
        public async Task<IActionResult> Index()
        {
            try
            {
                var lab2Context = _context.Livros
                    .Include(l => l.IdAutorNavigation)
                    .Include(l => l.IdBibliotecaNavigation)
                    .Include(l => l.UsernameBibAddNavigation);
                return View(await lab2Context.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar lista de livros: {ex.Message}");
                ModelState.AddModelError("", "Ocorreu um erro ao carregar a lista de livros.");
                return View(new List<Livro>());
            }

        }

        // GET: Livro/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdBibliotecaNavigation)
                .Include(l => l.UsernameBibAddNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livro/Create
        public IActionResult Create()
        {
            var usernameBib = User.Identity.Name;
            try
            {
                ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
                ViewBag.UsernameBibAdd = usernameBib;
                ViewBag.IdBiblioteca = "10001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dados do formulário de criação: {ex.Message}");
                ModelState.AddModelError("", "Ocorreu um erro ao carregar os dados necessários para criar o livro.");
            }
            return View();
        }

        // POST: Livro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Preco,Titulo,NExemplares,Genero,UsernameBibAdd,IdAutor,IdBiblioteca")] Livro livro)
        {
            var usernameBib = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                // Recarregar dropdowns em caso de erro
                ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
                ViewBag.UsernameBibAdd = usernameBib;
                ViewBag.IdBiblioteca = "10001";
            }

            try
            {
                // Verificar se a biblioteca existe antes de tentar salvar
                var bibliotecaExists = await _context.Bibliotecas.AnyAsync(b => b.IdBiblioteca == livro.IdBiblioteca);
                if (!bibliotecaExists)
                {
                    ModelState.AddModelError("", "A biblioteca selecionada não existe. Por favor, escolha uma biblioteca válida.");
                    ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
                    ViewBag.UsernameBibAdd = usernameBib;
                    ViewBag.IdBiblioteca = "10001";
                }

                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("FK__Livro__ID_Biblio"))
                {
                    ModelState.AddModelError("", "Erro de chave estrangeira: Certifique-se de que a biblioteca selecionada existe.");
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao salvar o livro. Tente novamente.");
                }
            }

            // Recarregar dropdowns para o caso de erro
            ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
            ViewBag.UsernameBibAdd = usernameBib;
            ViewBag.IdBiblioteca = "10001";
            return View(livro);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            var usernameBib = User.Identity.Name;
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdBibliotecaNavigation)
                .Include(l => l.UsernameBibAddNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);

            if (livro == null)
            {
                return NotFound();
            }

            ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
            ViewBag.UsernameBibAdd = usernameBib;
            ViewBag.IdBiblioteca = "10001";

            return View(livro);
        }


        // POST: Livro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Isbn,Preco,Titulo,NExemplares,Genero,UsernameBibAdd,IdAutor,IdBiblioteca")] Livro livro)
        {
            var usernameBib = User.Identity.Name;
            if (id != livro.Isbn)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Livro.IdAutorNavigation));
            ModelState.Remove(nameof(Livro.UsernameBibAddNavigation));
            ModelState.Remove(nameof(Livro.IdBibliotecaNavigation));
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo inválido.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Erro: {error.ErrorMessage}");
                }

                // Recarregar ViewBag para manter os dropdowns funcionais
                ViewBag.IdAutor = new SelectList(_context.Autors, "IdAutor", "NameAutor");
                ViewBag.UsernameBibAdd = usernameBib;
                ViewBag.IdBiblioteca = "10001";
                return View(livro);
            }

            try
            {
                // Atualiza o registro no banco de dados
                _context.Update(livro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se o registro ainda existe no banco de dados
                if (!LivroExists(livro.Isbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redireciona para a lista de livros após sucesso
            return RedirectToAction(nameof(Index));
        }

        // GET: Livro/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.IdAutorNavigation)
                .Include(l => l.IdBibliotecaNavigation)
                .Include(l => l.UsernameBibAddNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(long id)
        {
            return _context.Livros.Any(e => e.Isbn == id);
        }

        public async Task<IActionResult> marcarComoEntregue([FromBody] Requisicao req)
        {
            if (req == null || req.IdRequisicao <= 0)
            {
                return Json(new { sucesso = false, mensagem = "ID da requisição não fornecido ou inválido." });
            }

            var requisicao = await _context.Requisicaos
                .Include(r => r.IsbnNavigation)
                .FirstOrDefaultAsync(r => r.IdRequisicao == req.IdRequisicao);

            if (requisicao == null)
            {
                return Json(new { sucesso = false, mensagem = "Requisição não encontrada." });
            }

            // Verifica se o livro foi entregue antes da data de devolução
            if (DateTime.Now <= requisicao.DataDevolucao)
            {
                requisicao.DataDevolucao = DateTime.Now; // Usa DataRequisicao para armazenar a data de entrega
            }

            requisicao.Estado = false; // Marca como entregue

            var livro = requisicao.IsbnNavigation;
            if (livro != null)
            {
                livro.NExemplares += 1;
            }

            await _context.SaveChangesAsync();

            return Json(new { sucesso = true, mensagem = "Requisição marcada como entregue com sucesso." });
        }

        public async Task<IActionResult> History()
        {
            var historico = await _context.Requisicaos
                .Include(r => r.IsbnNavigation)
                .Include(r => r.UsernameLeiNavigation)
                .ToListAsync();

            return View(historico);
        }

        public async Task<IActionResult> EntregasAtraso()
        {
            var entregasatraso = await _context.Requisicaos
                .Where(r => r.DataDevolucao < DateTime.Now && r.Estado)
                .Include(r => r.IsbnNavigation)
                .Include(r => r.UsernameLeiNavigation)
                .ToListAsync();

            return View(entregasatraso);
        }
    }
}
