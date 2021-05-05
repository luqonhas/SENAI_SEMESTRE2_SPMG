using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Repositories;
using senai.spmg.webAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository;

        public UsuariosController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        private static ActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason}",
            ContentType = "text/plain",
        };

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_usuarioRepository.Listar());
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize]
        [HttpGet("perfil")]
        public IActionResult GetProfile()
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_usuarioRepository.ListarPerfil(idUsuario));
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Usuario usuarioBuscado = _usuarioRepository.BuscarPorId(id);

                if (usuarioBuscado == null)
                {
                    return NotFound("Nenhum usuário encontrado!");
                }

                return Ok(usuarioBuscado);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Post(Usuario novoUsuario)
        {
            try
            {
                Usuario usuarioEmail = _usuarioRepository.BuscarPorEmail(novoUsuario.Email);

                if (usuarioEmail == null)
                {
                    if (usuarioEmail == null)
                    {
                        _usuarioRepository.Cadastrar(novoUsuario);

                        return Result(HttpStatusCode.Created, $"Usuário com o email '{novoUsuario.Email}' cadastrado com sucesso!");
                    }
                }
                return BadRequest("Não foi possível cadastrar, e-mail já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, LoginViewModel usuarioAtualizado)
        {
            try
            {
                Usuario usuarioBuscado = _usuarioRepository.BuscarPorId(id);

                Usuario usuarioEmail = _usuarioRepository.BuscarPorEmail(usuarioAtualizado.email);

                if (usuarioBuscado != null)
                {
                    if (usuarioEmail == null)
                    {
                        usuarioBuscado = new Usuario
                        {
                            Email = usuarioAtualizado.email,
                            Senha = usuarioAtualizado.senha
                        };

                        _usuarioRepository.Atualizar(id, usuarioBuscado);

                        return StatusCode(204);
                    }
                }
                return BadRequest("Não foi possível atualizar, e-mail já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Usuario usuarioBuscado = _usuarioRepository.BuscarPorId(id);

                if (usuarioBuscado != null)
                {
                    _usuarioRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Usuário não encontrado!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

    }
}
