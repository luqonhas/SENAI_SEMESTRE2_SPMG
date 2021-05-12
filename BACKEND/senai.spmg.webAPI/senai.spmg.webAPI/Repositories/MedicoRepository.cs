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
    public class MedicoRepository : IMedicoRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar informações dos médicos com validações
        public void Atualizar(int id, Medico medicoAtualizado)
        {
            Medico medicoBuscado = BuscarPorId(id);

            Medico medicoBuscadoCRM = ctx.Medicos.FirstOrDefault(x => x.Crm == medicoAtualizado.Crm);

            if (medicoAtualizado.Crm != null && medicoBuscadoCRM == null)
            {
                medicoBuscado.Crm = medicoAtualizado.Crm;
            }

            if (medicoAtualizado.NomeMedico != null)
            {
                medicoBuscado.NomeMedico = medicoAtualizado.NomeMedico;
            }

            if (medicoAtualizado.IdUsuario != null && ctx.Usuarios.Find(medicoAtualizado.IdUsuario) != null)
            {
                medicoBuscado.IdUsuario = medicoAtualizado.IdUsuario;
            }

            if (medicoAtualizado.IdEspecialidade != null && ctx.Especialidades.Find(medicoAtualizado.IdEspecialidade) != null)
            {
                medicoBuscado.IdEspecialidade = medicoAtualizado.IdEspecialidade;
            }

            if (medicoAtualizado.IdClinica != null && ctx.Clinicas.Find(medicoAtualizado.IdClinica) != null)
            {
                medicoBuscado.IdClinica = medicoAtualizado.IdClinica;
            }

            ctx.Medicos.Update(medicoBuscado);

            ctx.SaveChanges();
        }

        // MVP - Método de buscar médicos por ID
        public Medico BuscarPorId(int id)
        {
            return ctx.Medicos.Include(x => x.Consulta).FirstOrDefault(x => x.IdMedico == id);
        }

        // MVP - Método de buscar CRM dos médicos para complementar outros métodos
        public Medico BuscarPorCRM(string crm)
        {
            Medico medicoBuscado = ctx.Medicos.FirstOrDefault(x => x.Crm == crm);

            if (medicoBuscado != null)
            {
                return medicoBuscado;
            }

            return null;
        }

        // MVP - Método de cadastrar novos médicos
        public void Cadastrar(Medico novoMedico)
        {
            ctx.Medicos.Add(novoMedico);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar médicos
        public void Deletar(int id)
        {
            ctx.Medicos.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todos os médicos
        public List<Medico> Listar()
        {
            return ctx.Medicos.Include(x => x.Consulta).ToList();
        }

        // MVP - Método de listar todos os médicos com suas consultas
        // MÉTODO NÃO NECESSÁRIO!
        /*
        public List<Medico> ListarConsultas(int id)
        {
            return ctx.Medicos
                .Include(x => x.Consulta)
                .Where(x => x.IdUsuario == id)
                .ToList();
        }
        */
    }
}
