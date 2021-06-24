using Microsoft.AspNetCore.Authorization;
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

        // MVP - Método para listar
        [Authorize(Roles = "1")]
        [HttpGet("todos")]
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

        // MVP - Método para listar por ID
        [Authorize(Roles = "1")]
        [HttpGet("buscar/{id}")]
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

        // MVP - Método para cadastrar
        [Authorize(Roles = "1")]
        [HttpPost("cadastrar")]
        public IActionResult Post(TipoUsuario novoTipo)
        {
            try
            {
                TipoUsuario permissao = _tipoRepository.BuscarPorPermissao(novoTipo.Permissao);

                if (permissao == null)
                {
                    _tipoRepository.Cadastrar(novoTipo);

                    return Created(HttpStatusCode.Created.ToString(), $"Permissão '{novoTipo.Permissao}' cadastrada com sucesso!");
                }
                return BadRequest("Não foi possível cadastrar, permissão já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para atualizar todas as informações
        [Authorize(Roles = "1")]
        [HttpPut("atualizar/{id}")]
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

        // MVP - Método para deletar
        [Authorize(Roles = "1")]
        [HttpDelete("deletar/{id}")]
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
