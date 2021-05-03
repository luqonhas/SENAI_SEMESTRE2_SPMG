using System;
using System.Collections.Generic;

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
        public string Permissao { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
