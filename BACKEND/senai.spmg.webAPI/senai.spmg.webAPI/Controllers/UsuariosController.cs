using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Models;
using senai.spmg.webAPI.Repositories;
using senai.spmg.webAPI.Services;
using senai.spmg.webAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository;
        private readonly IMailService _mailService;

        public UsuariosController(IMailService mailService)
        {
            _usuarioRepository = new UsuarioRepository();
            _mailService = mailService;
        }

        // MVP - Método para listar
        [Authorize(Roles = "1")]
        [HttpGet("todos")]
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

        // EXTRA - Método para listar informações do usuário logado
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

        // MVP - Método para listar por ID
        [Authorize(Roles = "1")]
        [HttpGet("buscar/{id}")]
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

        // MVP & EXTRA - Método para cadastrar e retonar um email de boas vindas
        [Authorize(Roles = "1")]
        [HttpPost("cadastrar")]
        public IActionResult Post(Usuario novoUsuario, [FromForm]WelcomeRequest request, string emailUser)
        {
            try
            {
                Usuario usuarioEmail = _usuarioRepository.BuscarPorEmail(novoUsuario.Email);

                if (usuarioEmail == null)
                {
                    if (usuarioEmail == null)
                    {
                        emailUser = novoUsuario.Email;

                        _mailService.SendWelcomeEmailAsync(request, emailUser);

                        _usuarioRepository.Cadastrar(novoUsuario);

                        return Created(HttpStatusCode.Created.ToString(), $"Usuário com o email '{novoUsuario.Email}' cadastrado com sucesso!");
                    }
                }
                return BadRequest("Não foi possível cadastrar, e-mail já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para atualizar parte das informações
        [Authorize(Roles = "1")]
        [HttpPatch("editarLogin/{id}")]
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

        // MVP - Método para deletar
        [Authorize(Roles = "1")]
        [HttpDelete("deletar/{id}")]
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
