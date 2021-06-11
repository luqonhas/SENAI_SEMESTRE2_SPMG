using Microsoft.EntityFrameworkCore;
using senai.spmg.webAPI.Contexts;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar informações das consultas com validações
        public void Atualizar(int id, Consulta consultaAtualizada)
        {
            Consulta consultaBuscada = BuscarPorId(id);

            if (consultaAtualizada.IdMedico != null && ctx.Medicos.Find(consultaAtualizada.IdMedico) != null)
            {
                consultaBuscada.IdMedico = consultaAtualizada.IdMedico;
            }

            if (consultaAtualizada.IdPaciente != null && ctx.Pacientes.Find(consultaAtualizada.IdPaciente) != null)
            {
                consultaBuscada.IdPaciente = consultaAtualizada.IdPaciente;
            }

            if (consultaAtualizada.IdSituacao != null && ctx.Situacoes.Find(consultaAtualizada.IdSituacao) != null)
            {
                consultaBuscada.IdSituacao = consultaAtualizada.IdSituacao;
            }

            if (consultaAtualizada.HoraConsulta.ToString() != "00:00:00")
            {
                consultaBuscada.HoraConsulta = consultaAtualizada.HoraConsulta;
            }

            if (consultaAtualizada.DataConsulta != Convert.ToDateTime("0001-01-01"))
            {
                consultaBuscada.DataConsulta = consultaAtualizada.DataConsulta;
            }

            consultaBuscada.Descricao = consultaAtualizada.Descricao;

            ctx.Consultas.Update(consultaBuscada);

            ctx.SaveChanges();
        }

        // MVP - Método de buscar consultas por ID
        public Consulta BuscarPorId(int id)
        {
            return ctx.Consultas.FirstOrDefault(x => x.IdConsulta == id);
        }

        // MVP - Método de buscar por situação das consultas para complementar outros métodos
        public Consulta BuscarPorSituacao(int id)
        {
            return ctx.Consultas.FirstOrDefault(x => x.IdSituacao == id);
        }

        // MVP - Método de cadastrar novas consultas
        public void Cadastrar(Consulta novaConsulta)
        {
            novaConsulta.IdSituacao = 3;

            ctx.Consultas.Add(novaConsulta);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar consultas
        public void Deletar(int id)
        {
            ctx.Consultas.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todas as consultas
        public List<Consulta> Listar()
        {
            return ctx.Consultas
                .Include(x => x.IdMedicoNavigation)
                .Include(x => x.IdMedicoNavigation.IdEspecialidadeNavigation)
                .Select(x => new Consulta
                {
                    IdConsulta = x.IdConsulta,
                    IdMedicoNavigation = x.IdMedicoNavigation,
                    IdPacienteNavigation = x.IdPacienteNavigation,
                    IdSituacaoNavigation = x.IdSituacaoNavigation,
                    Descricao = x.Descricao,
                    DataConsulta = x.DataConsulta,
                    HoraConsulta = x.HoraConsulta
                })
                .ToList();
        }

        // MVP - Método que atualiza os status/situação das consultas para 1 (Realizada), 2 (Cancelada) e 3 (Agendada)
        public void AtualizarSituacao(int idConsulta, int idSituacao)
        {
            Consulta consultaBuscada = ctx.Consultas.Find(idConsulta);

            switch (idSituacao)
            {
                case 1:
                    // realizada
                    consultaBuscada.IdSituacao = 1;
                    break;

                case 2:
                    // cancelada
                    consultaBuscada.IdSituacao = 2;
                    break;

                case 3:
                    // agendada
                    consultaBuscada.IdSituacao = 3;
                    break;

                default:
                    consultaBuscada.IdSituacao = consultaBuscada.IdSituacao;
                    break;
            }

            ctx.Consultas.Update(consultaBuscada);

            ctx.SaveChanges();
        }

        // MVP - Método para inserir/editar uma descrição nas consultas
        public void InserirDescricao(int id, Consulta descricao, int idUsuario)
        {
            Consulta consultaBuscada = ctx.Consultas.FirstOrDefault(x => x.IdConsulta == id);

            Medico medicoBuscado = ctx.Medicos.FirstOrDefault(x => x.IdUsuario == idUsuario);

            if (descricao.Descricao != null && consultaBuscada.IdMedico == medicoBuscado.IdMedico)
            {
                consultaBuscada.Descricao = descricao.Descricao;
            }

            ctx.Consultas.Update(consultaBuscada);

            ctx.SaveChanges();
        }

        // MVP - Método de listar as consultas do usuário logado
        public List<Consulta> ListarMinhasConsultas(int id)
        {
            Paciente pacienteBuscado = ctx.Pacientes.FirstOrDefault(x => x.IdUsuario == id);

            Medico medicoBuscado = ctx.Medicos.FirstOrDefault(x => x.IdUsuario == id);

            if (pacienteBuscado != null)
            {
                return ctx.Consultas.Where(x => x.IdPaciente == pacienteBuscado.IdPaciente)
                    .Include(x => x.IdPacienteNavigation)
                    .Include(x => x.IdMedicoNavigation)
                    .Include(x => x.IdSituacaoNavigation)
                    .Include(x => x.IdMedicoNavigation.IdEspecialidadeNavigation)
                    .Select(x => new Consulta
                    {
                        IdConsulta = x.IdConsulta,
                        IdMedicoNavigation = x.IdMedicoNavigation,
                        IdPacienteNavigation = x.IdPacienteNavigation,
                        IdSituacaoNavigation = x.IdSituacaoNavigation,
                        Descricao = x.Descricao,
                        DataConsulta = x.DataConsulta,
                        HoraConsulta = x.HoraConsulta
                    })
                    .ToList();
            }

            if (medicoBuscado != null)
            {
                return ctx.Consultas.Include(x => x.IdMedicoNavigation).Where(x => x.IdMedico == medicoBuscado.IdMedico)
                    .Include(x => x.IdPacienteNavigation)
                    .Include(x => x.IdMedicoNavigation)
                    .Include(x => x.IdSituacaoNavigation)
                    .Include(x => x.IdMedicoNavigation.IdEspecialidadeNavigation)
                    .Select(x => new Consulta
                    {
                        IdConsulta = x.IdConsulta,
                        IdMedicoNavigation = x.IdMedicoNavigation,
                        IdPacienteNavigation = x.IdPacienteNavigation,
                        IdSituacaoNavigation = x.IdSituacaoNavigation,
                        Descricao = x.Descricao,
                        DataConsulta = x.DataConsulta,
                        HoraConsulta = x.HoraConsulta
                    })
                    .ToList();
            }

            return null;
        }


    }
}
