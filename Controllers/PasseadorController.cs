using System;
using System.Threading.Tasks;
using DogWalkr.Database;
using DogWalkr.Repositories;
using DogWalkr.ViewModels.Passeador;
using DogWalkr.ViewModels.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogWalkr.Controllers
{
    public class PasseadorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DogWalkrDb _database;

        public PasseadorController(IWebHostEnvironment webHostEnvironment, DogWalkrDb database)
        {
            _webHostEnvironment = webHostEnvironment;
            _database = database;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(PasseadorCadastrarViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Id = Guid.NewGuid();
                viewModel.WebRootPath = _webHostEnvironment.WebRootPath;

                await _database.Connection.OpenAsync();

                var repository = new PasseadorRepository(_database);

                await repository.Create(viewModel);

                HttpContext.Session.SetString("userId", viewModel.Id.ToString());
                HttpContext.Session.SetString("username", viewModel.Nome);
                HttpContext.Session.SetString("role", "PASSEADOR");

                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public async Task<IActionResult> Editar()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new PasseadorRepository(_database);

            var passeador = await repository.Get(Guid.Parse(passeadorId));

            var viewModel = new PasseadorEditarViewModel
            {
                Id = passeador.Id,
                Nome = passeador.Nome,
                Login = passeador.Login,
                SenhaSalva = passeador.Senha,
                Email = passeador.Email,
                Telefone = passeador.Telefone,
                Nascimento = passeador.Nascimento,
                Endereco = passeador.Endereco,
                Cep = passeador.Cep,
                Cidade = passeador.Cidade,
                Estado = passeador.Estado,
                Descricao = passeador.Descricao,
                FotoSalva = passeador.Foto,
                Horario = passeador.Horario,
                Qualificacoes = passeador.Qualificacoes,
                DistanciaMaxima = passeador.DistanciaMaxima
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PasseadorEditarViewModel viewModel)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            if (ModelState.IsValid)
            {
                viewModel.WebRootPath = _webHostEnvironment.WebRootPath;

                await _database.Connection.OpenAsync();

                var repository = new PasseadorRepository(_database);

                await repository.Update(viewModel);

                TempData["sucesso"] = "Alterações salvas com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public IActionResult Entrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _database.Connection.OpenAsync();

                var repository = new PasseadorRepository(_database);

                var passeador = await repository.Get(viewModel.Login, viewModel.Senha);

                if (passeador == null)
                {
                    ViewData["error"] = "Não conseguimos encontrar o usuário.";
                    return View();
                }
                else
                {
                    HttpContext.Session.SetString("userId", passeador.Id.ToString());
                    HttpContext.Session.SetString("username", passeador.Nome);
                    HttpContext.Session.SetString("role", "PASSEADOR");

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewData["error"] = "Verifique os dados inseridos.";
            return View();
        }

        public IActionResult Sair()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Curtir(Guid cachorroId, Guid usuarioId)
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            await _database.Connection.OpenAsync();

            var repository = new PasseadorRepository(_database);

            string response = await repository.Like(Guid.Parse(passeadorId), cachorroId, usuarioId);

            if (response == "MATCH")
            {
                TempData["curtir"] = "Parabéns, você teve um match!";
            }
            else
            {
                TempData["curtir"] = "Você curtiu!";
            }

            return RedirectToAction("Procurar", "Cachorro");
        }

        [HttpPost]
        public async Task<IActionResult> Ignorar(Guid cachorroId, Guid usuarioId)
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            await _database.Connection.OpenAsync();

            var repository = new PasseadorRepository(_database);

            await repository.Ignore(Guid.Parse(passeadorId), cachorroId, usuarioId);

            TempData["ignorar"] = "Esse cachorro não aparecerá novamente.";
            return RedirectToAction("Procurar", "Cachorro");
        }

        public async Task<IActionResult> Procurar()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new PasseadorRepository(_database);

            var passeadores = await repository.Search(Guid.Parse(usuarioId));

            return View(passeadores);
        }

        public async Task<IActionResult> ListarMatches()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            await _database.Connection.OpenAsync();

            var repository = new PasseadorRepository(_database);

            var usuarios = await repository.GetMatches(Guid.Parse(passeadorId));

            return View(usuarios);
        }
    }
}