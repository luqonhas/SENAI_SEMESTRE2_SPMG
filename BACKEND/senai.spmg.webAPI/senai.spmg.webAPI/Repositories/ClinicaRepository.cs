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
    public class ClinicaRepository : IClinicaRepository
    {
        SPMGContext ctx = new SPMGContext();

        public void Atualizar(int id, Clinica clinicaAtualizada)
        {
            Clinica clinicaBuscada = BuscarPorId(id);

            Clinica clinicaBuscadaCNPJ = ctx.Clinicas.FirstOrDefault(x => x.Cnpj == clinicaAtualizada.Cnpj);

            Clinica clinicaBuscadaFantasia = ctx.Clinicas.FirstOrDefault(x => x.NomeFantasia == clinicaAtualizada.NomeFantasia);

            Clinica clinicaBuscadaRazao = ctx.Clinicas.FirstOrDefault(x => x.RazaoSocial == clinicaAtualizada.RazaoSocial);

            if (clinicaAtualizada.Cnpj != null && clinicaBuscadaCNPJ == null)
            {
                clinicaBuscada.Cnpj = clinicaAtualizada.Cnpj;
            }
            if (clinicaAtualizada.NomeFantasia != null && clinicaBuscadaFantasia == null)
            {
                clinicaBuscada.NomeFantasia = clinicaAtualizada.NomeFantasia;
            }
            if (clinicaAtualizada.RazaoSocial != null && clinicaBuscadaRazao == null)
            {
                clinicaBuscada.RazaoSocial = clinicaAtualizada.RazaoSocial;
            }
            if (clinicaAtualizada.Endereco != null)
            {
                clinicaBuscada.Endereco = clinicaAtualizada.Endereco;
            }

            clinicaBuscada.HorarioAbertura = clinicaAtualizada.HorarioAbertura;

            clinicaBuscada.HorarioFechamento = clinicaAtualizada.HorarioFechamento;

            ctx.Clinicas.Update(clinicaBuscada);

            ctx.SaveChanges();
        }

        public Clinica BuscarPorId(int id)
        {
            return ctx.Clinicas.Include(x => x.Medicos).FirstOrDefault(x => x.IdClinica == id);
        }

        public Clinica BuscarPorCNPJ(string cnpj)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.Cnpj == cnpj);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        public Clinica BuscarPorFantasia(string fantasia)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.NomeFantasia == fantasia);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        public Clinica BuscarPorRazao(string razao)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.RazaoSocial == razao);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        public void Cadastrar(Clinica novaClinica)
        {
            ctx.Clinicas.Add(novaClinica);

            ctx.SaveChanges();
        }

        public void Deletar(int id)
        {
            ctx.Clinicas.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        public List<Clinica> Listar()
        {
            return ctx.Clinicas.Include(x => x.Medicos).ToList();
        }


    }
}
