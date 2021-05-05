using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Repositories
{
    interface ISituacaoRepository
    {
        List<Situacoes> Listar();

        Situacoes BuscarPorId(int id);
    }
}
