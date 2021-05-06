using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Repositories;
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
    public class PacientesController : ControllerBase
    {
        private IPacienteRepository _pacienteRepository;

        public PacientesController()
        {
            _pacienteRepository = new PacienteRepository();
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
                return Ok(_pacienteRepository.Listar());
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet("consultas")]
        public IActionResult GetConsult()
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_pacienteRepository.ListarConsultas(idUsuario));
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
                Paciente pacienteBuscado = _pacienteRepository.BuscarPorId(id);

                if (pacienteBuscado == null)
                {
                    return NotFound("Nenhum paciente encontrado!");
                }

                return Ok(pacienteBuscado);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPost]
        public IActionResult Post(Paciente novoPaciente)
        {
            try
            {
                Paciente pacienteCPF = _pacienteRepository.BuscarPorCPF(novoPaciente.Cpf);

                Paciente pacienteRG = _pacienteRepository.BuscarPorRG(novoPaciente.Rg);

                if (novoPaciente.DataNascimento.Year >= DateTime.Now.Year)
                {
                    return BadRequest("Insira uma data de nascimento válida");
                }

                if (pacienteCPF == null)
                {
                    if (pacienteRG == null)
                    {
                        if (pacienteCPF == null && pacienteRG == null)
                        {
                            _pacienteRepository.Cadastrar(novoPaciente);

                            return Result(HttpStatusCode.Created, $"Paciente '{novoPaciente.NomePaciente}' cadastrado com sucesso!");
                        }
                    }
                    return BadRequest("Não foi possível cadastrar, RG já existente!");
                }
                return BadRequest("Não foi possível cadastrar, CPF já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Paciente pacienteAtualizado)
        {
            try
            {
                Paciente pacienteBuscado = _pacienteRepository.BuscarPorId(pacienteAtualizado.IdPaciente);

                if (pacienteBuscado != null)
                {
                    Paciente pacienteCPF = _pacienteRepository.BuscarPorCPF(pacienteAtualizado.Cpf);

                    Paciente pacienteRG = _pacienteRepository.BuscarPorRG(pacienteAtualizado.Rg);

                    if (pacienteCPF == null)
                    {
                        if (pacienteRG == null)
                        {
                            if (pacienteCPF == null && pacienteRG == null)
                            {
                                _pacienteRepository.Atualizar(id, pacienteAtualizado);

                                return StatusCode(204);
                            }
                        }
                        return BadRequest("Não foi possível atualizar, RG já existente!");
                    }
                    return BadRequest("Não foi possível atualizar, CPF já existente!");
                }
                return BadRequest("Nenhum paciente encontrado!");
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
                Paciente pacienteBuscado = _pacienteRepository.BuscarPorId(id);

                if (pacienteBuscado != null)
                {
                    _pacienteRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Paciente não encontrado!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }



    }
}
