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
    public class MedicosController : ControllerBase
    {
        private IMedicoRepository _medicoRepository;

        public MedicosController()
        {
            _medicoRepository = new MedicoRepository();
        }

        // MVP - Método para listar
        [Authorize(Roles = "1, 2")]
        [HttpGet("todos")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_medicoRepository.Listar());
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
                Medico medicoBuscado = _medicoRepository.BuscarPorId(id);

                if (medicoBuscado == null)
                {
                    return NotFound("Nenhum médico encontrado!");
                }

                return Ok(medicoBuscado);
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

                return Ok(_medicoRepository.ListarPerfil(idUsuario));
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para cadastrar
        [Authorize(Roles = "1")]
        [HttpPost("cadastrar")]
        public IActionResult Post(Medico novoMedico)
        {
            try
            {
                Medico medicoCRM = _medicoRepository.BuscarPorCRM(novoMedico.Crm);

                if (medicoCRM == null)
                {
                    _medicoRepository.Cadastrar(novoMedico);

                    return Created(HttpStatusCode.Created.ToString(), $"Médico '{novoMedico.NomeMedico}' cadastrado com sucesso!");
                }
                return BadRequest("Não foi possível cadastrar, CRM já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para atualizar todas as informações
        [Authorize(Roles = "1")]
        [HttpPut("atualizar/{id}")]
        public IActionResult Put(int id, Medico medicoAtualizado)
        {
            try
            {
                Medico medicoBuscado = _medicoRepository.BuscarPorId(id);

                if (medicoBuscado != null)
                {
                    Medico medicoCRM = _medicoRepository.BuscarPorCRM(medicoAtualizado.Crm);

                    if (medicoCRM == null)
                    {
                        _medicoRepository.Atualizar(id, medicoAtualizado);

                        return StatusCode(204);
                    }
                    return BadRequest("Não foi possível cadastrar, CRM já existente!");
                }
                return NotFound("Médico não encontrado!");
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
                Medico medicoBuscado = _medicoRepository.BuscarPorId(id);

                if (medicoBuscado != null)
                {
                    _medicoRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Médico não encontrado!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }


    }
}
