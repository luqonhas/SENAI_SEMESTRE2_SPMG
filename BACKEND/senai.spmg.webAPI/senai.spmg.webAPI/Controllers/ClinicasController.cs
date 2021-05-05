using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Repositories;
using senai.spmg.webAPI.ViewModel;
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
    public class ClinicasController : ControllerBase
    {
        private IClinicaRepository _clinicaRepository;

        public ClinicasController()
        {
            _clinicaRepository = new ClinicaRepository();
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
                return Ok(_clinicaRepository.Listar());
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
                Clinica clinicaBuscada = _clinicaRepository.BuscarPorId(id);

                if (clinicaBuscada == null)
                {
                    return NotFound("Nenhuma clínica encontrada!");
                }

                return Ok(clinicaBuscada);
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Post(Clinica novaClinica)
        {
            try
            {
                Clinica clinicaCNPJ = _clinicaRepository.BuscarPorCNPJ(novaClinica.Cnpj);

                Clinica clinicaFantasia = _clinicaRepository.BuscarPorFantasia(novaClinica.NomeFantasia);

                Clinica clinicaRazao = _clinicaRepository.BuscarPorRazao(novaClinica.RazaoSocial);

                if (clinicaCNPJ == null)
                {
                    if (clinicaFantasia == null)
                    {
                        if (clinicaRazao == null)
                        {
                            if (clinicaCNPJ == null && clinicaFantasia == null && clinicaRazao == null)
                            {
                                _clinicaRepository.Cadastrar(novaClinica);

                                return Result(HttpStatusCode.Created, $"Clínica '{novaClinica.NomeFantasia}' cadastrada com sucesso!");
                            }      
                        }
                        return BadRequest("Não foi possível cadastrar, razão social já existente!");
                    }
                    return BadRequest("Não foi possível cadastrar, nome fantasia já existente!");
                }
                return BadRequest("Não foi possível cadastrar, CNPJ já existente!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, Clinica clinicaAtualizada)
        {
            try
            {
                Clinica clinicaBuscada = _clinicaRepository.BuscarPorId(id);

                if (clinicaBuscada != null)
                {
                    Clinica clinicaCNPJ = _clinicaRepository.BuscarPorCNPJ(clinicaAtualizada.Cnpj);

                    Clinica clinicaFantasia = _clinicaRepository.BuscarPorFantasia(clinicaAtualizada.NomeFantasia);

                    Clinica clinicaRazao = _clinicaRepository.BuscarPorRazao(clinicaAtualizada.RazaoSocial);

                    if (clinicaCNPJ == null)
                    {
                        if (clinicaFantasia == null)
                        {
                            if (clinicaRazao == null)
                            {
                                if (clinicaCNPJ == null && clinicaFantasia == null && clinicaRazao == null)
                                {
                                    _clinicaRepository.Atualizar(id, clinicaAtualizada);

                                    return StatusCode(204);
                                }
                            }
                            return BadRequest("Não foi possível cadastrar, razão social já existente!");
                        }
                        return BadRequest("Não foi possível cadastrar, nome fantasia já existente!");
                    }
                    return BadRequest("Não foi possível cadastrar, CNPJ já existente!");
                }
                return NotFound("Clínica não encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, ClinicaViewModel clinicaAtualizada)
        {
            try
            {
                Clinica clinicaBuscada = _clinicaRepository.BuscarPorId(id);

                if (clinicaBuscada != null)
                {
                    Clinica clinicaFantasia = _clinicaRepository.BuscarPorFantasia(clinicaAtualizada.NomeFantasia);

                    Clinica clinicaRazao = _clinicaRepository.BuscarPorRazao(clinicaAtualizada.RazaoSocial);

                    if (clinicaFantasia == null)
                    {
                        if (clinicaRazao == null)
                        {
                            if (clinicaFantasia == null && clinicaRazao == null)
                            {
                                clinicaBuscada = new Clinica
                                {
                                    NomeFantasia = clinicaAtualizada.NomeFantasia
                                };

                                _clinicaRepository.Atualizar(id, clinicaBuscada);

                                return StatusCode(204);
                            }
                        }
                        return BadRequest("Não foi possível cadastrar, razão social já existente!");
                    }
                    return BadRequest("Não foi possível cadastrar, nome fantasia já existente!");
                }
                return NotFound("Clínica não encontrada!");
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
                Clinica clinicaBuscada = _clinicaRepository.BuscarPorId(id);

                if (clinicaBuscada != null)
                {
                    _clinicaRepository.Deletar(id);

                    return StatusCode(204);
                }

                return NotFound("Clínica não encontrada!");
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }


    }
}
