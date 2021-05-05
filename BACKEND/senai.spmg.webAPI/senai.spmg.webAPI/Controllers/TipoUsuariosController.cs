using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuariosController : ControllerBase
    {
        private ITipoUsuarioRepository _tipoRepository;

        public TipoUsuariosController()
        {
            _tipoRepository = new TipoUsuarioRepository();
        }

        private static ActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason}",
            ContentType = "text/plain",
        };

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_tipoRepository.Listar());
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                TipoUsuario tipoBuscado = _tipoRepository.BuscarPorId(id);

                if (tipoBuscado == null)
                {
                    return NotFound("Nenhuma permissão encontrada!");
                }

                return Ok(tipoBuscado);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPost]
        public IActionResult Post(TipoUsuario novoTipo)
        {
            try
            {
                TipoUsuario permissao = _tipoRepository.BuscarPorPermissao(novoTipo.Permissao);

                if (permissao == null)
                {
                    _tipoRepository.Cadastrar(novoTipo);

                    return Result(HttpStatusCode.Created, $"Permissão '{novoTipo.Permissao}' cadastrada com sucesso!");
                }
                return BadRequest("Não foi possível cadastrar, permissão já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, TipoUsuario tipoAtualizado)
        {
            try
            {
                TipoUsuario tipoBuscado = _tipoRepository.BuscarPorId(id);

                if (tipoAtualizado != null)
                {
                    TipoUsuario permissao = _tipoRepository.BuscarPorPermissao(tipoAtualizado.Permissao);

                    if (permissao == null)
                    {
                        _tipoRepository.Atualizar(id, tipoAtualizado);

                        return StatusCode(204);
                    }
                    return BadRequest("Não foi possível cadastrar, permissão já existente!");
                }
                return NotFound("Permissão não encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                TipoUsuario tipoBuscado = _tipoRepository.BuscarPorId(id);

                if (tipoBuscado != null)
                {
                    _tipoRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Permissão não encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

    }
}
