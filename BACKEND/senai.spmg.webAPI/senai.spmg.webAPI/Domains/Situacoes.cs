using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace senai.spmg.webAPI.Domains
{
    public partial class Situacoes
    {
        public Situacoes()
        {
            Consulta = new HashSet<Consulta>();
        }

        public int IdSituacao { get; set; }

        [Required(ErrorMessage = "Campo 'situacao' obrigatório!")]
        public string Situacao { get; set; }

        public virtual ICollection<Consulta> Consulta { get; set; }
    }
}
