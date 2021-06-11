using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using senai.spmg.webAPI.Repositories;
using senai.spmg.webAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUsuarioRepository _loginRepository;

        public LoginController()
        {
            _loginRepository = new UsuarioRepository();
        }

        // MVP - Método para logar e gerar o token
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                // busca o usuário pelo email e senha
                Usuario usuarioBuscado = _loginRepository.Logar(login.email, login.senha);

                // se os dados estiverem errados...
                if (usuarioBuscado == null)
                {
                    return NotFound("E-mail ou senha inválidos!");
                }

                // se os dados estiverem certos e serem encontrados, prossegue para o token

                /*
                DEPENDÊNCIAS DO JWT (Pacotes):

                Criar e validar o JWT:      System.IdentityModel.Tokens.Jwt
                Integrar a autenticação:    Microsoft.AspNetCore.Authentication.JwtBearer (versão compatível com o .NET SDK do projeto)
                */

                // define os dados que serão fornecidos no token - o PAYLOAD
                var claims = new[]
                {
                    // armazena na Claim, o email do usuário, aquele que foi buscado
                    new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),

                    // armazena na Claim, o ID do usuário autenticado
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.IdUsuario.ToString()),

                    // armazena na Claim, o tipo de usuário que foi autenticado ("Administrador")
                    // aqui a Role é o título da permissão
                    // new Claim(ClaimTypes.Role, usuarioBuscado.IdTipoUsuarioNavigation.TituloTipoUsuario)

                    // Armazena na Claim, o tipo de usuário que foi autenticado ("1")
                    //new Claim(ClaimTypes.Role, usuarioBuscado.IdTipoUsuarioNavigation.Permissao),
                    new Claim(ClaimTypes.Role, usuarioBuscado.IdTipoUsuario.ToString()),

                    // Armazena na Claim, o tipo de usuário que foi autenticado ("1") de forma personalizada
                    new Claim("role", usuarioBuscado.IdTipoUsuario.ToString())
                    };

                // define a chave secreta ao token
                var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("spmg-chave-autenticacao"));

                // define as credenciais do token, a sua assinatura
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var dadosToken = new JwtSecurityToken(
                    issuer: "spmg.webAPI",                      // emissor do token
                    audience: "spmg.webAPI",                    // destinatário do token
                    claims: claims,                             // dados definidos acima
                    expires: DateTime.Now.AddMinutes(30),       // tempo de expiração
                    signingCredentials: credentials             // credenciais do token
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(dadosToken)
                });

            }
            catch (Exception codErro)
            {
                return BadRequest(codErro);
            }

        }
    }
}
