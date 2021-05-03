using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace senai.spmg.webAPI.Domains
{
    public partial class Clinica
    {
        public Clinica()
        {
            Medicos = new HashSet<Medico>();
        }

        public int IdClinica { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "O CNPJ deve conter apenas números!")]
        [StringLength(maximumLength: 14, MinimumLength = 14, ErrorMessage = "O CNPJ precisa ter exatos 14 números!")]
        [Required(ErrorMessage = "Campo 'cnpj' obrigatório!")]
        public string Cnpj { get; set; }

        [StringLength(maximumLength: 200, MinimumLength = 1, ErrorMessage = "O nome da empresa inserida é muito grande!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'nomeFantasia' obrigatório!")]
        public string NomeFantasia { get; set; }

        [StringLength(maximumLength: 200, MinimumLength = 1, ErrorMessage = "A razão social inserida é muito grande!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'razaoSocial' obrigatório!")]
        public string RazaoSocial { get; set; }

        [StringLength(maximumLength: 250, MinimumLength = 1, ErrorMessage = "O endereço inserido é muito grande!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'endereco' obrigatório!")]
        public string Endereco { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'horarioAbertura' obrigatório!")]
        public TimeSpan HorarioAbertura { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'horarioFechamento' obrigatório!")]
        public TimeSpan HorarioFechamento { get; set; }

        public virtual ICollection<Medico> Medicos { get; set; }
    }
}
