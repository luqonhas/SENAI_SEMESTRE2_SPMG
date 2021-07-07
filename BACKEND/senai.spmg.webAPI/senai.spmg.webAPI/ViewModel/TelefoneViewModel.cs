using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class TelefoneViewModel
    {
        [Required(ErrorMessage = "Campo 'telefonePaciente' obrigatório!")]
        public string TelefonePaciente { get; set; }
    }
}
