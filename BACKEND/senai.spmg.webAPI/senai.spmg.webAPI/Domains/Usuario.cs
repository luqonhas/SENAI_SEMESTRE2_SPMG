using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace senai.spmg.webAPI.Domains
{
    public partial class Usuario
    {
        public Usuario()
        {
            Medicos = new HashSet<Medico>();
            Pacientes = new HashSet<Paciente>();
        }

        public int IdUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "É necessário que o usuário tenha um nível de permissão!")]
        public int? IdTipoUsuario { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "O valor inserido não é um e-mail válido!")]
        [StringLength(maximumLength: 200, MinimumLength = 10, ErrorMessage = "O e-mail inserido é muito curto ou muito longo!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'email' obrigatório!")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 200, MinimumLength = 6, ErrorMessage = "A senha inserida é muito curta ou muito longa!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'senha' obrigatório!")]
        public string Senha { get; set; }
        public string Foto { get; set; }

        public virtual TipoUsuario IdTipoUsuarioNavigation { get; set; }
        public virtual ICollection<Medico> Medicos { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
