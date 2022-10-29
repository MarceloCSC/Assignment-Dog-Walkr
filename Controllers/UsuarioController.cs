using System;
using System.Threading.Tasks;
using DogWalkr.Database;
using DogWalkr.Repositories;
using DogWalkr.ViewModels.Shared;
using DogWalkr.ViewModels.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogWalkr.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DogWalkrDb _database;

        public UsuarioController(DogWalkrDb database)
        {
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
        public async Task<IActionResult> Cadastrar(UsuarioEditarViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Id = Guid.NewGuid();

                await _database.Connection.OpenAsync();

                var repository = new UsuarioRepository(_database);

                await repository.Create(viewModel);

                HttpContext.Session.SetString("userId", viewModel.Id.ToString());
                HttpContext.Session.SetString("username", viewModel.Nome);
                HttpContext.Session.SetString("role", "USUARIO");

                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(viewModel);
        }

        public async Task<IActionResult> Editar()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new UsuarioRepository(_database);

            var usuario = await repository.Get(Guid.Parse(usuarioId));

            var viewModel = new UsuarioEditarViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Login = usuario.Login,
                SenhaSalva = usuario.Senha,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Nascimento = usuario.Nascimento,
                Endereco = usuario.Endereco,
                Cep = usuario.Cep,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioEditarViewModel usuario)
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            if (ModelState.IsValid)
            {
                await _database.Connection.OpenAsync();

                var repository = new UsuarioRepository(_database);

                await repository.Update(usuario);

                TempData["sucesso"] = "Alterações salvas com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Ops, parece que tivemos um problema!";
            return View(usuario);
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

                var repository = new UsuarioRepository(_database);

                var usuario = await repository.Get(viewModel.Login, viewModel.Senha);

                if (usuario == null)
                {
                    ViewData["error"] = "Não conseguimos encontrar o usuário.";
                    return View();
                }
                else
                {
                    HttpContext.Session.SetString("userId", usuario.Id.ToString());
                    HttpContext.Session.SetString("username", usuario.Nome);
                    HttpContext.Session.SetString("role", "USUARIO");

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
        public async Task<IActionResult> Curtir(Guid id)
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new UsuarioRepository(_database);

            string response = await repository.Like(Guid.Parse(usuarioId), id);

            if (response == "MATCH")
            {
                TempData["curtir"] = "Parabéns, você teve um match!";
            }
            else
            {
                TempData["curtir"] = "Você curtiu!";
            }

            return RedirectToAction("Procurar", "Passeador");
        }

        [HttpPost]
        public async Task<IActionResult> Ignorar(Guid id)
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new UsuarioRepository(_database);

            await repository.Ignore(Guid.Parse(usuarioId), id);

            TempData["ignorar"] = "Esse passeador não aparecerá novamente.";
            return RedirectToAction("Procurar", "Passeador");
        }

        public async Task<IActionResult> ListarMatches()
        {
            string usuarioId = HttpContext.Session.GetString("userId");

            if (usuarioId == null)
            {
                return RedirectToAction("Entrar", "Usuario");
            }

            await _database.Connection.OpenAsync();

            var repository = new UsuarioRepository(_database);

            var passeadores = await repository.GetMatches(Guid.Parse(usuarioId));

            return View(passeadores);
        }
    }
}