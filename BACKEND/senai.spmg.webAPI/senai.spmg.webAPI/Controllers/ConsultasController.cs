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
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private IConsultaRepository _consultaRepository;

        public ConsultasController()
        {
            _consultaRepository = new ConsultaRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_consultaRepository.Listar());
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
                Consulta consultaBuscada = _consultaRepository.BuscarPorId(id);

                if (consultaBuscada == null)
                {
                    return NotFound("Nenhuma consulta encontrada!");
                }

                return Ok(consultaBuscada);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPost]
        public IActionResult Post(Consulta novaConsulta)
        {
            try
            {
                _consultaRepository.Cadastrar(novaConsulta);

                return StatusCode(201);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPost("agendar")]
        public IActionResult PostConsult(ConsultaViewModel novaConsulta)
        {
            try
            {
                Consulta consultaSituacao = _consultaRepository.BuscarPorSituacao(novaConsulta.idSituacao);

                if (consultaSituacao != null)
                {
                    Consulta consultaBuscada = new Consulta
                    {
                        IdPaciente = novaConsulta.idPaciente,
                        IdMedico = novaConsulta.idMedico,
                        DataConsulta = novaConsulta.dataAgendamento,
                        HoraConsulta = novaConsulta.horaAgendamento,
                        IdSituacao = novaConsulta.idSituacao
                    };

                    _consultaRepository.Cadastrar(consultaBuscada);

                    return StatusCode(201);
                }
                return BadRequest("Nenhuma situação encontrada!");    
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("agendamentos/situacao/{id}")]
        public IActionResult PatchSit(int id, ConsultaViewModel situacaoAtualizada)
        {
            try
            {
                Consulta consultaBuscada = _consultaRepository.BuscarPorId(id);

                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                if (consultaBuscada != null)
                {
                    if (situacaoAtualizada.idSituacao != 1 || situacaoAtualizada.idSituacao != 2 || situacaoAtualizada.idSituacao != 3)
                    {
                        if (situacaoAtualizada.idSituacao == 2 || situacaoAtualizada.idSituacao == 3)
                        {
                            if (situacaoAtualizada.idSituacao == 2 || situacaoAtualizada.idSituacao == 3)
                            {
                                if (situacaoAtualizada.idSituacao == 1 && idUsuario != 1 || idUsuario != 2 || situacaoAtualizada.idSituacao == 3 && idUsuario != 1 || idUsuario != 2)
                                {
                                    return NotFound("Somente usuários com a permissão de 'Administrador' ou 'Médico' podem marcar uma consulta como realizada ou agendada!");
                                }

                                consultaBuscada = new Consulta
                                {
                                    IdSituacao = situacaoAtualizada.idSituacao
                                };

                                _consultaRepository.Atualizar(id, consultaBuscada);

                                return StatusCode(204);
                            }

                            if (situacaoAtualizada.idSituacao == 2 && idUsuario != 1)
                            {
                                return NotFound("Somente usuários com a permissão de 'Administrador' podem marcar uma consulta como cancelada!");
                            }

                            consultaBuscada = new Consulta
                            {
                                IdSituacao = situacaoAtualizada.idSituacao
                            };

                            _consultaRepository.Atualizar(id, consultaBuscada);

                            return StatusCode(204);
                        }
                    }
                    return BadRequest("Situação inserida não existente!");
                }
                return BadRequest("Nenhuma consulta encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Consulta consultaAtualizada)
        {
            try
            {
                _consultaRepository.Atualizar(id, consultaAtualizada);

                return StatusCode(204);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Médico")]
        [HttpPatch("{id}")]
        public IActionResult PatchDesc(int id, ConsultaViewModel descricaoAtualizado)
        {
            try
            {
                Consulta consultaBuscada = _consultaRepository.BuscarPorId(id);

                if (consultaBuscada != null)
                {
                    consultaBuscada = new Consulta
                    {
                        Descricao = descricaoAtualizado.descricao
                    };

                    _consultaRepository.Atualizar(id, consultaBuscada);

                    return StatusCode(204);
                }
                return BadRequest("Nenhuma consulta encontrada!");
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
                _consultaRepository.Deletar(id);

                return StatusCode(204);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }


    }
}
