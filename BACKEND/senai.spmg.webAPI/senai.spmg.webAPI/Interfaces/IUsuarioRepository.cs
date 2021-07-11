using Microsoft.AspNetCore.Http;
using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IUsuarioRepository
    {
        List<Usuario> Listar();

        List<Usuario> ListarSemSenha();

        Usuario BuscarPorId(int id);

        Usuario BuscarPorEmail(string email);

        void Cadastrar(Usuario novoUsuario);

        void Atualizar(int id, Usuario usuarioAtualizado);

        void Deletar(int id);

        Usuario Logar(string email, string senha);

        List<Usuario> ListarPerfil(int id);

        string BuscarNomePorId(int id);

        int BuscarPacientePorId(int id);

        bool AlterarEmail(int id, Usuario email);

        string UploadFoto(IFormFile arquivo, string savingFolder);

        string AlterarFoto(IFormFile novaFoto, int idUsuario);

    }
}
