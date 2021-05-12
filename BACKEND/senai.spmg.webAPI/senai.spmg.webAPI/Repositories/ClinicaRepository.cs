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

        // MVP - Método de atualizar informações das clínicas com validações
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

            if (clinicaAtualizada.HorarioAbertura.ToString() != "00:00:00")
            {
                clinicaBuscada.HorarioAbertura = clinicaAtualizada.HorarioAbertura;
            }

            if (clinicaAtualizada.HorarioFechamento.ToString() != "00:00:00")
            {
                clinicaBuscada.HorarioFechamento = clinicaAtualizada.HorarioFechamento;
            }

            ctx.Clinicas.Update(clinicaBuscada);

            ctx.SaveChanges();
        }

        // MVP - Método de buscar clínicas por ID
        public Clinica BuscarPorId(int id)
        {
            return ctx.Clinicas.FirstOrDefault(x => x.IdClinica == id);
        }

        // MVP - Método de buscar CNPJ das clínicas para complementar outros métodos
        public Clinica BuscarPorCNPJ(string cnpj)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.Cnpj == cnpj);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        // MVP - Método de buscar por nome fantasia das clínicas para complementar outros métodos
        public Clinica BuscarPorFantasia(string fantasia)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.NomeFantasia == fantasia);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        // MVP - Método de buscar por razão social das clínicas para complementar outros métodos
        public Clinica BuscarPorRazao(string razao)
        {
            Clinica clinicaBuscada = ctx.Clinicas.FirstOrDefault(x => x.RazaoSocial == razao);

            if (clinicaBuscada != null)
            {
                return clinicaBuscada;
            }

            return null;
        }

        // MVP - Método de cadastrar novas clínicas
        public void Cadastrar(Clinica novaClinica)
        {
            ctx.Clinicas.Add(novaClinica);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar clínicas
        public void Deletar(int id)
        {
            ctx.Clinicas.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todas as clínicas
        public List<Clinica> Listar()
        {
            return ctx.Clinicas.Include(x => x.Medicos).ToList();
        }


    }
}
