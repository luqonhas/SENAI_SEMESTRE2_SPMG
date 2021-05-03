using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IClinicaRepository
    {
        List<Clinica> Listar();

        Clinica BuscarPorId(int id);

        Clinica BuscarPorCNPJ(string cnpj);

        Clinica BuscarPorFantasia(string fantasia);

        Clinica BuscarPorRazao(string razao);

        void Cadastrar(Clinica novaClinica);

        void Atualizar(int id, Clinica clinicaAtualizada);

        void Deletar(int id);
    }
}
