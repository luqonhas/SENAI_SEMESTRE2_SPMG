using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IMedicoRepository
    {
        List<Medico> Listar();

        Medico BuscarPorId(int id);

        Medico BuscarPorCRM(string crm);

        void Cadastrar(Medico novoMedico);

        void Atualizar(int id, Medico medicoAtualizado);

        void Deletar(int id);

        List<Medico> ListarConsultas(int id);
    }
}
