using System;
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

        public CachorroController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Cadastrar(CachorroCadastrarViewModel viewModel)
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

                var repository = new CachorroRepository();

                repository.Create(viewModel);

                ModelState.Clear();

                ViewData["success"] = "Muito bem, você cadastrou o seu dog com sucesso!";
                return View();
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public IActionResult Listar()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            var repository = new CachorroRepository();

            var lista = repository.GetAll(Guid.Parse(usuarioId));

            return View(lista);
        }

        public IActionResult Editar(Guid id)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            var repository = new CachorroRepository();

            var cachorro = repository.Get(id);

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
        public IActionResult Editar(CachorroEditarViewModel viewModel)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            if (ModelState.IsValid)
            {
                viewModel.WebRootPath = _webHostEnvironment.WebRootPath;

                var repository = new CachorroRepository();

                repository.Update(viewModel);

                TempData["sucesso"] = "Alterações salvas com sucesso!";
                return RedirectToAction("Listar");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Apagar(Guid id)
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            var repository = new CachorroRepository();

            repository.Delete(id, Guid.Parse(usuarioId));

            return RedirectToAction("Listar");
        }

        public IActionResult Procurar()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            var repository = new CachorroRepository();

            var cachorros = repository.Search(Guid.Parse(passeadorId));

            return View(cachorros);
        }
    }
}