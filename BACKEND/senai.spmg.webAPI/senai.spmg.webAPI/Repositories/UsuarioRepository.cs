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
    public class UsuarioRepository : IUsuarioRepository
    {
        SPMGContext ctx = new SPMGContext();

        public void Atualizar(int id, Usuario usuarioAtualizado)
        {
            Usuario usuarioBuscado = BuscarPorId(id);

            Usuario usuarioBuscadoEmail = ctx.Usuarios.FirstOrDefault(x => x.Email == usuarioAtualizado.Email);

            if (usuarioAtualizado.Email != null && usuarioBuscadoEmail == null)
            {
                usuarioBuscado.Email = usuarioAtualizado.Email;
            }
            if (usuarioAtualizado.Senha != null)
            {
                usuarioBuscado.Senha = usuarioAtualizado.Senha;
            }
            if (usuarioAtualizado.IdTipoUsuario != null)
            {
                usuarioBuscado.IdTipoUsuario = usuarioAtualizado.IdTipoUsuario;
            }

            ctx.Usuarios.Update(usuarioBuscado);

            ctx.SaveChanges();
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario usuarioBuscado = ctx.Usuarios.FirstOrDefault(x => x.Email == email);

            if (usuarioBuscado != null)
            {
                return usuarioBuscado;
            }

            return null;
        }

        public Usuario BuscarPorId(int id)
        {
            return ctx.Usuarios.Include(x => x.Pacientes).Include(x => x.Medicos).FirstOrDefault(x => x.IdUsuario == id);
        }

        public void Cadastrar(Usuario novoUsuario)
        {
            ctx.Usuarios.Add(novoUsuario);

            ctx.SaveChanges();
        }

        public void Deletar(int id)
        {
            ctx.Usuarios.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        public List<Usuario> Listar()
        {
            return ctx.Usuarios.Include(x => x.Pacientes).Include(x => x.Medicos).ToList();
        }

        public List<Usuario> ListarPerfil(int id)
        {
            return ctx.Usuarios
                .Include(x => x.IdTipoUsuarioNavigation)
                .Include(x => x.Medicos)
                .Include(x => x.Pacientes)
                .Where(x => x.IdUsuario == id)
                .ToList();
        }

        public List<Usuario> ListarSemSenha()
        {
            var usuarioSemSenha = ctx.Usuarios
            .Include(x => x.IdTipoUsuarioNavigation)
            .Select(x => new Usuario()
            {
                IdUsuario = x.IdUsuario,
                Email = x.Email,
                IdTipoUsuario = x.IdTipoUsuario,
                IdTipoUsuarioNavigation = x.IdTipoUsuarioNavigation
            })
            .Where(x => x.IdTipoUsuarioNavigation.IdTipoUsuario == 2 && x.IdTipoUsuarioNavigation.IdTipoUsuario == 3);

            return usuarioSemSenha.ToList();
        }

        public Usuario Logar(string email, string senha)
        {
            Usuario login = ctx.Usuarios.Include(x => x.IdTipoUsuarioNavigation).FirstOrDefault(x => x.Email == email && x.Senha == senha);

            return login;
        }

    }
}
