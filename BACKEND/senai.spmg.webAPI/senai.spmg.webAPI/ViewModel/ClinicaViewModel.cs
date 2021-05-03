using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class ClinicaViewModel
    {
        [Required(ErrorMessage = "Campo 'nomeFantasia' obrigatório!")]
        public string NomeFantasia { get; set; }

        public string RazaoSocial { get; set; }

        public string Endereco { get; set; }

        public TimeSpan HorarioAbertura { get; set; }

        public TimeSpan HorarioFechamento { get; set; }
    }
}
