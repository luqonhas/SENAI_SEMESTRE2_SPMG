using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Campo 'email' obrigatório!")]
        public string Email { get; set; }
    }
}
