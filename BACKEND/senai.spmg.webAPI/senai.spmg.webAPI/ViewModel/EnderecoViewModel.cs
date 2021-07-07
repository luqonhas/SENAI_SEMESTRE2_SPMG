using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class EnderecoViewModel
    {
        [Required(ErrorMessage = "Campo 'endereco' obrigatório!")]
        public string Endereco { get; set; }
    }
}
