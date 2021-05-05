using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace senai.spmg.webAPI.Domains
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdTipoUsuario { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "A permissão inserida é muito grande!")]
        [RegularExpression("^[a-zA-Z ç ~ ã õ ê â î ô ñ û ú í á é ó ü ï ä ö ë]+$", ErrorMessage = "A permissão deve conter apenas letras!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'permissao' obrigatório!")]
        public string Permissao { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
