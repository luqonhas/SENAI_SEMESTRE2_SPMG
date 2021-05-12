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
    public class EspecialidadeRepository : IEspecialidadeRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar o nome das especialidades com validações
        public bool Atualizar(int id, Especialidade especialidadeAtualizado)
        {
            Especialidade especialidadeBuscada = BuscarPorId(id);

            Especialidade especialidadeBuscar = ctx.Especialidades.FirstOrDefault(x => x.nomeEspecialidade == especialidadeAtualizado.nomeEspecialidade);

            if (especialidadeAtualizado.nomeEspecialidade != null && especialidadeBuscar == null)
            {
                especialidadeBuscada.nomeEspecialidade = especialidadeAtualizado.nomeEspecialidade;

                ctx.Especialidades.Update(especialidadeBuscada);

                ctx.SaveChanges();

                return true;
            }
            return false;
        }

        // MVP - Método de buscar especialidades por ID
        public Especialidade BuscarPorId(int id)
        {
            return ctx.Especialidades.Include(x => x.Medicos).FirstOrDefault(x => x.IdEspecialidade == id);
        }

        // MVP - Método de buscar por nome das especialidades para complementar outros métodos
        public Especialidade BuscarPorEspecialidade(string especialidade)
        {
            Especialidade especialidadeBuscada = ctx.Especialidades.FirstOrDefault(x => x.nomeEspecialidade == especialidade);

            if (especialidadeBuscada != null)
            {
                return especialidadeBuscada;
            }

            return null;
        }

        // MVP - Método de cadastrar novas especialidades
        public void Cadastrar(Especialidade novoEspecialidade)
        {
            ctx.Especialidades.Add(novoEspecialidade);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar especialidades
        public void Deletar(int id)
        {
            ctx.Especialidades.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todas as especialidades
        public List<Especialidade> Listar()
        {
            return ctx.Especialidades.Include(x => x.Medicos).ToList();
        }


    }
}
