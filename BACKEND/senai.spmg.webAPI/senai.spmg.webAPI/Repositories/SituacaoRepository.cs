using senai.spmg.webAPI.Contexts;
using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Repositories
{
    public class SituacaoRepository : ISituacaoRepository
    {
        SPMGContext ctx = new SPMGContext();

        public Situacoes BuscarPorId(int id)
        {
            return ctx.Situacoes.FirstOrDefault(x => x.IdSituacao == id);
        }

        public List<Situacoes> Listar()
        {
            return ctx.Situacoes.ToList();
        }

    }
}
