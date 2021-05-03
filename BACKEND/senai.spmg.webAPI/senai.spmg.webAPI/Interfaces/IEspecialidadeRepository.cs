using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IEspecialidadeRepository
    {
        List<Especialidade> Listar();

        Especialidade BuscarPorId(int id);

        Especialidade BuscarPorNome(string especialidade);

        void Cadastrar(Especialidade novoEspecialidade);

        bool Atualizar(int id, Especialidade especialidadeAtualizado);

        void Deletar(int id);
    }
}
