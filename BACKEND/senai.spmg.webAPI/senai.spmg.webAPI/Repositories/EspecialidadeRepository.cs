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

        public bool Atualizar(int id, Especialidade especialidadeAtualizado)
        {
            Especialidade especialidadeBuscada = BuscarPorId(id);

            Especialidade especialidadeBuscar = ctx.Especialidades.FirstOrDefault(x => x.Especialidade1 == especialidadeAtualizado.Especialidade1);

            if (especialidadeAtualizado.Especialidade1 != null && especialidadeBuscar == null)
            {
                especialidadeBuscada.Especialidade1 = especialidadeAtualizado.Especialidade1;

                ctx.Especialidades.Update(especialidadeBuscada);

                ctx.SaveChanges();

                return true;
            }
            return false;
        }

        public Especialidade BuscarPorId(int id)
        {
            return ctx.Especialidades.Include(x => x.Medicos).FirstOrDefault(x => x.IdEspecialidade == id);
        }

        public Especialidade BuscarPorEspecialidade(string especialidade)
        {
            Especialidade especialidadeBuscada = ctx.Especialidades.FirstOrDefault(x => x.Especialidade1 == especialidade);

            if (especialidadeBuscada != null)
            {
                return especialidadeBuscada;
            }

            return null;
        }

        public void Cadastrar(Especialidade novoEspecialidade)
        {
            ctx.Especialidades.Add(novoEspecialidade);

            ctx.SaveChanges();
        }

        public void Deletar(int id)
        {
            ctx.Especialidades.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        public List<Especialidade> Listar()
        {
            return ctx.Especialidades.Include(x => x.Medicos).ToList();
        }


    }
}
