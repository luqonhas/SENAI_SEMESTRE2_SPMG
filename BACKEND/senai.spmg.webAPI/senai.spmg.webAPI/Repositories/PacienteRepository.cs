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
    public class PacienteRepository : IPacienteRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar informações dos pacientes com validações
        public void Atualizar(int id, Paciente pacienteAtualizado)
        {
            Paciente pacienteBuscado = BuscarPorId(id);

            Paciente pacienteBuscadoCPF = ctx.Pacientes.FirstOrDefault(x => x.Cpf == pacienteAtualizado.Cpf);

            Paciente pacienteBuscadoRG = ctx.Pacientes.FirstOrDefault(x => x.Rg == pacienteAtualizado.Rg);

            if (pacienteAtualizado.Cpf != null && pacienteBuscadoCPF == null)
            {
                pacienteBuscado.Cpf = pacienteAtualizado.Cpf;
            }

            if (pacienteAtualizado.Rg != null && pacienteBuscadoRG == null)
            {
                pacienteBuscado.Rg = pacienteAtualizado.Rg;
            }

            if (pacienteAtualizado.Endereco != null)
            {
                pacienteBuscado.Endereco = pacienteAtualizado.Endereco;
            }

            if (pacienteAtualizado.NomePaciente != null)
            {
                pacienteBuscado.NomePaciente = pacienteAtualizado.NomePaciente;
            }

            if (pacienteAtualizado.IdUsuario != null && ctx.Usuarios.Find(pacienteAtualizado.IdUsuario) != null)
            {
                pacienteBuscado.IdUsuario = pacienteAtualizado.IdUsuario;
            }

            if (pacienteAtualizado.TelefonePaciente != null)
            {
                pacienteBuscado.TelefonePaciente = pacienteAtualizado.TelefonePaciente;
            }

            if (pacienteAtualizado.DataNascimento != Convert.ToDateTime("0001-01-01"))
            {
                pacienteBuscado.DataNascimento = pacienteAtualizado.DataNascimento;
            }

            ctx.Pacientes.Update(pacienteBuscado);

            ctx.SaveChanges();
        }

        // MVP - Método de buscar pacientes por ID
        public Paciente BuscarPorId(int id)
        {
            return ctx.Pacientes.Include(x => x.Consulta).FirstOrDefault(x => x.IdPaciente == id);
        }

        // MVP - Método de buscar CPF dos pacientes para complementar outros métodos
        public Paciente BuscarPorCPF(string cpf)
        {
            Paciente pacienteBuscado = ctx.Pacientes.FirstOrDefault(x => x.Cpf == cpf);

            if (pacienteBuscado != null)
            {
                return pacienteBuscado;
            }

            return null;
        }

        // MVP - Método de buscar RG dos pacientes para complementar outros métodos
        public Paciente BuscarPorRG(string rg)
        {
            Paciente pacienteBuscado = ctx.Pacientes.FirstOrDefault(x => x.Rg == rg);

            if (pacienteBuscado != null)
            {
                return pacienteBuscado;
            }

            return null;
        }

        // MVP - Método de cadastrar novos pacientes
        public void Cadastrar(Paciente novoPaciente)
        {
            ctx.Pacientes.Add(novoPaciente);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar pacientes
        public void Deletar(int id)
        {
            ctx.Pacientes.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todos os pacientes
        public List<Paciente> Listar()
        {
            return ctx.Pacientes.Include(x => x.Consulta).ToList();
        }

        // MVP - Método de listar todas os pacientes com suas consultas
        // MÉTODO NÃO NECESSÁRIO!
        /*
        public List<Paciente> ListarConsultas(int id)
        {
            return ctx.Pacientes
                .Include(x => x.Consulta)
                .Where(x => x.IdUsuario == id)
                .ToList();
        }
        */


    }
}
