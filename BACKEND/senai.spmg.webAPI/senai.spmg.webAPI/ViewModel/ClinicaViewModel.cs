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

        [Required(ErrorMessage = "Campo 'razaoSocial' obrigatório!")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "Campo 'cnpj' obrigatório!")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Campo 'endereco' obrigatório!")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Campo 'horarioAbertura' obrigatório!")]
        public TimeSpan HorarioAbertura { get; set; }

        [Required(ErrorMessage = "Campo 'horarioFechamento' obrigatório!")]
        public TimeSpan HorarioFechamento { get; set; }
    }
}
