using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface ITipoUsuarioRepository
    {
        List<TipoUsuario> Listar();

        TipoUsuario BuscarPorId(int id);

        TipoUsuario BuscarPorPermissao(string permissao);

        void Cadastrar(TipoUsuario novoTipo);

        bool Atualizar(int id, TipoUsuario tipoAtualizado);

        void Deletar(int id);
    }
}
