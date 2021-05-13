using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Models
{
    public class WelcomeRequest
    {
        // email do usuário que se registrou
        public string ToEmail { get; set; }

        // usuário que se registrou
        public string UserName { get; set; }
    }
}
