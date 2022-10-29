using System;
using System.Threading.Tasks;
using DogWalkr.Database;
using DogWalkr.Repositories;
using DogWalkr.ViewModels.Cachorro;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogWalkr.Controllers
{
    public class CachorroController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DogWalkrDb _database;

        public CachorroController(IWebHostEnvironment webHostEnvironment, DogWalkrDb database)
        {
            _webHostEnvironment = webHostEnvironment;
            _database = database;
        }

        public IActionResult Cadastrar()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CachorroCadastrarViewModel viewModel)
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            if (ModelState.IsValid)
            {
                viewModel.Id = Guid.NewGuid();
                viewModel.UsuarioId = Guid.Parse(usuarioId);
                viewModel.WebRootPath = _webHostEnvironment.WebRootPath;

                await _database.Connection.OpenAsync();

                var repository = new CachorroRepository(_database);

                await repository.Create(viewModel);

                ModelState.Clear();

                ViewData["success"] = "Muito bem, você cadastrou o seu dog com sucesso!";
                return View();
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public async Task<IActionResult> Listar()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new CachorroRepository(_database);

            var lista = await repository.GetAll(Guid.Parse(usuarioId));

            return View(lista);
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new CachorroRepository(_database);

            var cachorro = await repository.Get(id);

            var viewModel = new CachorroEditarViewModel
            {
                Id = id,
                UsuarioId = cachorro.UsuarioId,
                Nome = cachorro.Nome,
                Raca = cachorro.Raca,
                Porte = cachorro.Porte,
                Idade = cachorro.Idade,
                Sexo = cachorro.Sexo,
                Descricao = cachorro.Descricao,
                FotoSalva = cachorro.Foto
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CachorroEditarViewModel viewModel)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            if (ModelState.IsValid)
            {
                viewModel.WebRootPath = _webHostEnvironment.WebRootPath;

                await _database.Connection.OpenAsync();

                var repository = new CachorroRepository(_database);

                await repository.Update(viewModel);

                TempData["sucesso"] = "Alterações salvas com sucesso!";
                return RedirectToAction("Listar");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Apagar(Guid id)
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new CachorroRepository(_database);

            await repository.Delete(id, Guid.Parse(usuarioId));

            return RedirectToAction("Listar");
        }

        public async Task<IActionResult> Procurar()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            await _database.Connection.OpenAsync();

            var repository = new CachorroRepository(_database);

            var cachorros = await repository.Search(Guid.Parse(passeadorId));

            return View(cachorros);
        }
    }
}