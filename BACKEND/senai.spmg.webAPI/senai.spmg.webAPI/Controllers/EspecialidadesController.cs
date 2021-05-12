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

        // MVP - Método para listar
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

        // MVP - Método para listar por ID
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

        // MVP - Método para cadastrar e retornar os dados cadastrados
        [HttpPost]
        public IActionResult Post(Especialidade novaEspecialidade)
        {
            try
            {
                Especialidade especialidade = _especialidadeRepository.BuscarPorEspecialidade(novaEspecialidade.nomeEspecialidade);

                if (especialidade == null)
                {
                    _especialidadeRepository.Cadastrar(novaEspecialidade);

                    return Created(HttpStatusCode.Created.ToString(), novaEspecialidade);
                }
                return BadRequest("Não foi possível cadastrar, especialidade já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para atualizar todas as informações
        [HttpPut("{id}")]
        public IActionResult Put(int id, Especialidade especialidadeAtualizada)
        {
            try
            {
                Especialidade especialidadeBuscada = _especialidadeRepository.BuscarPorId(id);

                if (especialidadeAtualizada != null)
                {
                    Especialidade especialidade = _especialidadeRepository.BuscarPorEspecialidade(especialidadeAtualizada.nomeEspecialidade);

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

        // MVP - Método para deletar
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
