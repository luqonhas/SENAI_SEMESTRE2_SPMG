using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.ViewModel
{
    public class SituacaoViewModel
    {
        [Required(ErrorMessage = "Campo 'idSituacao' obrigatório!")]
        public int IdSituacao { get; set; }
    }
}
