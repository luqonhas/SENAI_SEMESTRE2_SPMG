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
    public class EspecialidadesController : ControllerBase
    {
        private IEspecialidadeRepository _especialidadeRepository;

        public EspecialidadesController()
        {
            _especialidadeRepository = new EspecialidadeRepository();
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
                return Ok(_especialidadeRepository.Listar());
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
                Especialidade especialidadeBuscada = _especialidadeRepository.BuscarPorId(id);

                if (especialidadeBuscada == null)
                {
                    return NotFound("Nenhuma especialidade encontrada!");
                }

                return Ok(especialidadeBuscada);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPost]
        public IActionResult Post(Especialidade novaEspecialidade)
        {
            try
            {
                Especialidade especialidade = _especialidadeRepository.BuscarPorEspecialidade(novaEspecialidade.Especialidade1);

                if (especialidade == null)
                {
                    _especialidadeRepository.Cadastrar(novaEspecialidade);

                    return Result(HttpStatusCode.Created, $"Especialidade '{novaEspecialidade.Especialidade1}' cadastrada com sucesso!");
                }
                return BadRequest("Não foi possível cadastrar, especialidade já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Especialidade especialidadeAtualizada)
        {
            try
            {
                Especialidade especialidadeBuscada = _especialidadeRepository.BuscarPorId(id);

                if (especialidadeAtualizada != null)
                {
                    Especialidade especialidade = _especialidadeRepository.BuscarPorEspecialidade(especialidadeAtualizada.Especialidade1);

                    if (especialidade == null)
                    {
                        _especialidadeRepository.Atualizar(id, especialidadeAtualizada);

                        return StatusCode(204);
                    }
                    return BadRequest("Não foi possível cadastrar, especialidade já existente!");
                }
                return NotFound("Especialidade não encontrada!");
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
                Especialidade especialidadeBuscada = _especialidadeRepository.BuscarPorId(id);

                if (especialidadeBuscada != null)
                {
                    _especialidadeRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Especialidade não encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }


    }
}
