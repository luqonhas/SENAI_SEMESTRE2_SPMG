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

        List<Paciente> ListarPerfil(int id);

        bool AlterarTelefone(int id, Paciente telefone);

        bool AlterarEndereco(int id, Paciente endereco);

        Paciente BuscarUsuarioPorId(int idUsuario);

        Paciente BuscarPorTelefone(string telefone);

        // MÉTODO NÃO NECESSÁRIO!
        // List<Paciente> ListarConsultas(int id);
    }
}
