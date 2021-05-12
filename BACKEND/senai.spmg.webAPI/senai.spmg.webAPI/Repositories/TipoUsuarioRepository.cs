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
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar informações dos tipos usuários com validações
        public bool Atualizar(int id, TipoUsuario tipoAtualizado)
        {
            TipoUsuario tipoBuscada = BuscarPorId(id);

            TipoUsuario permissaoBuscar = ctx.TipoUsuarios.FirstOrDefault(x => x.Permissao == tipoAtualizado.Permissao);

            if (tipoAtualizado.Permissao != null && permissaoBuscar == null)
            {
                tipoBuscada.Permissao = tipoAtualizado.Permissao;

                ctx.TipoUsuarios.Update(tipoBuscada);

                ctx.SaveChanges();

                return true;
            }
            return false;
        }

        // MVP - Método de buscar tipos de usuários por ID
        public TipoUsuario BuscarPorId(int id)
        {
            return ctx.TipoUsuarios.Include(x => x.Usuarios).FirstOrDefault(x => x.IdTipoUsuario == id);
        }

        // MVP - Método de buscar por permissão dos tipos usuários para complementar outros métodos
        public TipoUsuario BuscarPorPermissao(string permissao)
        {
            TipoUsuario tipoBuscado = ctx.TipoUsuarios.FirstOrDefault(x => x.Permissao == permissao);

            if (tipoBuscado != null)
            {
                return tipoBuscado;
            }

            return null;
        }

        // MVP - Método de cadastrar novos tipos usuários
        public void Cadastrar(TipoUsuario novoTipo)
        {
            ctx.TipoUsuarios.Add(novoTipo);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar tipos usuários
        public void Deletar(int id)
        {
            ctx.TipoUsuarios.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todos os tipos usuários
        public List<TipoUsuario> Listar()
        {
            return ctx.TipoUsuarios.Include(x => x.Usuarios).ToList();
        }


    }
}
