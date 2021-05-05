using senai.spmg.webAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Interfaces
{
    interface IPacienteRepository
    {
        List<Paciente> Listar();

        Paciente BuscarPorId(int id);

        Paciente BuscarPorCPF(string cpf);

        Paciente BuscarPorRG(string rg);

        void Cadastrar(Paciente novoPaciente);

        void Atualizar(int id, Paciente pacienteAtualizado);

        void Deletar(int id);

        List<Paciente> ListarConsultas(int id);
    }
}
