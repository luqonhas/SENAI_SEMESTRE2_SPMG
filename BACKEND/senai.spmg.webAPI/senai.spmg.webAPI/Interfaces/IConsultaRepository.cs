using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IConsultaRepository
    {
        List<Consulta> Listar();

        Consulta BuscarPorId(int id);

        void Cadastrar(Consulta novaConsulta);

        void Atualizar(int id, Consulta consultaAtualizada);

        void Deletar(int id);

        Consulta BuscarPorSituacao(int id);

        void AtualizarSituacao(int idConsulta, int idSituacao);

        void InserirDescricao(int id, Consulta descricao, int idUsuario);

        List<Consulta> ListarMinhasConsultas(int id);
    }
}
