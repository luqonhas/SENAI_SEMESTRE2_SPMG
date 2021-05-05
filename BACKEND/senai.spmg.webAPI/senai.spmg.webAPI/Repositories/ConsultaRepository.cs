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

        public void Atualizar(int id, Consulta consultaAtualizada)
        {
            Consulta consultaBuscada = BuscarPorId(id);

            if (consultaAtualizada.IdMedico != null)
            {
                consultaBuscada.IdMedico = consultaAtualizada.IdMedico;
            }
            if (consultaAtualizada.IdPaciente != null)
            {
                consultaBuscada.IdPaciente = consultaAtualizada.IdPaciente;
            }
            if (consultaAtualizada.IdSituacao != null)
            {
                consultaBuscada.IdSituacao = consultaAtualizada.IdSituacao;
            }
            if (consultaAtualizada.Descricao != null)
            {
                consultaBuscada.Descricao = consultaAtualizada.Descricao;
            }

            consultaBuscada.DataConsulta = consultaAtualizada.DataConsulta;

            consultaBuscada.HoraConsulta = consultaAtualizada.HoraConsulta;

            ctx.Consultas.Update(consultaBuscada);

            ctx.SaveChanges();
        }

        public Consulta BuscarPorId(int id)
        {
            return ctx.Consultas.FirstOrDefault(x => x.IdConsulta == id);
        }

        public Consulta BuscarPorSituacao(int id)
        {
            return ctx.Consultas.FirstOrDefault(x => x.IdSituacao == id);
        }

        public void Cadastrar(Consulta novaConsulta)
        {
            ctx.Consultas.Add(novaConsulta);

            ctx.SaveChanges();
        }

        public void Deletar(int id)
        {
            ctx.Consultas.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        public List<Consulta> Listar()
        {
            return ctx.Consultas.ToList();
        }

    }
}
