using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo 'email' obrigatório")]
        public string email { get; set; }

        [Required(ErrorMessage = "Campo 'senha' obrigatório")]
        public string senha { get; set; }
    }
}
