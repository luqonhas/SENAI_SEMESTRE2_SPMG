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
    public class PacientesController : ControllerBase
    {
        private IPacienteRepository _pacienteRepository;

        public PacientesController()
        {
            _pacienteRepository = new PacienteRepository();
        }

        // MVP - Método para listar
        [Authorize(Roles = "1")]
        [HttpGet("todos")]
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

        // MVP - Método para listar por ID
        [Authorize(Roles = "1")]
        [HttpGet("buscar/{id}")]
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

        [Authorize]
        [HttpGet("perfil")]
        public IActionResult GetProfile()
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_pacienteRepository.ListarPerfil(idUsuario));
            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }
        }

        // MVP - Método para cadastrar
        [Authorize]
        [HttpPost("cadastrar")]
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

                            return Created(HttpStatusCode.Created.ToString(), novoPaciente);
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

        // MVP - Método para atualizar todas as informações
        [Authorize]
        [HttpPut("atualizar/{id}")]
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

        [Authorize(Roles = "3")]
        [HttpPatch("telefone")]
        public IActionResult PatchPhone(TelefoneViewModel telefoneAtualizado)
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                Paciente pacienteBuscado = _pacienteRepository.BuscarUsuarioPorId(idUsuario);

                Paciente pacienteTelefone = _pacienteRepository.BuscarPorTelefone(telefoneAtualizado.TelefonePaciente);

                // return StatusCode(200, telefoneAtualizado);
                if (pacienteBuscado != null)
                {
                    if (telefoneAtualizado.TelefonePaciente != null && pacienteTelefone == null)
                    {
                        pacienteBuscado = new Paciente
                        {
                            TelefonePaciente = telefoneAtualizado.TelefonePaciente
                        };

                        _pacienteRepository.AlterarTelefone(idUsuario, pacienteBuscado);

                        return StatusCode(204);
                    }
                    return BadRequest("Telefone já utilizado!");
                }
                return BadRequest("Nenhum paciente encontrado!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "3")]
        [HttpPatch("endereco")]
        public IActionResult PatchEndereco(EnderecoViewModel enderecoAtualizado)
        {
            try
            {
                int idUsuario = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                Paciente pacienteBuscado = _pacienteRepository.BuscarUsuarioPorId(idUsuario);

                // return StatusCode(200, telefoneAtualizado);
                if (pacienteBuscado != null)
                {
                    if (enderecoAtualizado.Endereco != null)
                    {
                        pacienteBuscado = new Paciente
                        {
                            Endereco = enderecoAtualizado.Endereco
                        };

                        _pacienteRepository.AlterarEndereco(idUsuario, pacienteBuscado);

                        return StatusCode(204);
                    }
                }
                return BadRequest("Nenhum paciente encontrado!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // MVP - Método para deletar
        [Authorize]
        [HttpDelete("deletar/{id}")]
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
