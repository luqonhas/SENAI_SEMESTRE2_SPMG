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

        // MVP - Método para listar
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

        [Authorize(Roles = "Médico, Paciente")]
        [HttpGet("minhas")]
        public IActionResult GetMyConsults()
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_consultaRepository.ListarMinhasConsultas(idUsuario));
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para listar por ID
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

        // MVP - Método para cadastrar
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

        // EXTRA - Método para cadastrar um parte das informações
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

        // MVP - Método para atualizar todas as informações
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

        // MVP - Método para atualizar uma parte das informações (descrição)
        [Authorize(Roles = "Médico")]
        [HttpPatch("{id}")]
        public IActionResult PatchDesc(int id, ConsultaViewModel descricaoAtualizado)
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                Consulta consultaBuscada = _consultaRepository.BuscarPorId(id);

                if (consultaBuscada != null)
                {
                    consultaBuscada = new Consulta
                    {
                        Descricao = descricaoAtualizado.descricao
                    };

                    _consultaRepository.InserirDescricao(id, consultaBuscada, idUsuario);

                    return StatusCode(204);
                }
                return BadRequest("Nenhuma consulta encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para atualizar parte das informações (situação)
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}")]
        public IActionResult PatchSit(int id, SituacaoViewModel status)
        {
            try
            {
                Consulta consultaBuscada = _consultaRepository.BuscarPorId(id);

                if (consultaBuscada != null)
                {
                    _consultaRepository.AtualizarSituacao(id, status.IdSituacao);

                    return StatusCode(204);
                }
                return BadRequest("Nenhuma consulta encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para deletar
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
