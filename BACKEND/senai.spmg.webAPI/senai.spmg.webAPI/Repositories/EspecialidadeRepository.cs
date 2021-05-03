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
            throw new NotImplementedException();
        }

        public Especialidade BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Especialidade BuscarPorNome(string especialidade)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Especialidade novoEspecialidade)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Especialidade> Listar()
        {
            throw new NotImplementedException();
        }
    }
}
