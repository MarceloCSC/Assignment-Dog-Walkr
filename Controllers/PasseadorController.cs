using System;
using System.Threading.Tasks;
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

        public PasseadorController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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

                var repository = new PasseadorRepository();

                await repository.Create(viewModel);

                HttpContext.Session.SetString("userId", viewModel.Id.ToString());
                HttpContext.Session.SetString("username", viewModel.Nome);
                HttpContext.Session.SetString("role", "PASSEADOR");

                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public IActionResult Editar()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            var repository = new PasseadorRepository();

            var passeador = repository.Get(Guid.Parse(passeadorId));

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

                var repository = new PasseadorRepository();

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
        public IActionResult Entrar(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var repository = new PasseadorRepository();

                var passeador = repository.Get(viewModel.Login, viewModel.Senha);

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
        public IActionResult Curtir(Guid cachorroId, Guid usuarioId)
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            var repository = new PasseadorRepository();

            string response = repository.Like(Guid.Parse(passeadorId), cachorroId, usuarioId);

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
        public IActionResult Ignorar(Guid cachorroId, Guid usuarioId)
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            var repository = new PasseadorRepository();

            repository.Ignore(Guid.Parse(passeadorId), cachorroId, usuarioId);

            TempData["ignorar"] = "Esse cachorro não aparecerá novamente.";
            return RedirectToAction("Procurar", "Cachorro");
        }

        public IActionResult Procurar()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            var repository = new PasseadorRepository();

            var passeadores = repository.Search(Guid.Parse(usuarioId));

            return View(passeadores);
        }

        public IActionResult ListarMatches()
        {
            string passeadorId = HttpContext.Session.GetString("userId");

            if (passeadorId == null)
            {
                return RedirectToAction("Entrar", "Passeador");
            }

            var repository = new PasseadorRepository();

            var usuarios = repository.GetMatches(Guid.Parse(passeadorId));

            return View(usuarios);
        }
    }
}