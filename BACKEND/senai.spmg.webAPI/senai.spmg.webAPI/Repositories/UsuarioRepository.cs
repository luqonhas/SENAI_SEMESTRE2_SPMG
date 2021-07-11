using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using senai.spmg.webAPI.Contexts;
using senai.spmg.webAPI.Domains;
using senai.spmg.webAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        SPMGContext ctx = new SPMGContext();

        // MVP - Método de atualizar informações dos usuários com validações
        public void Atualizar(int id, Usuario usuarioAtualizado)
        {
            Usuario usuarioBuscado = BuscarPorId(id);

            Usuario usuarioBuscadoEmail = ctx.Usuarios.FirstOrDefault(x => x.Email == usuarioAtualizado.Email);

            if (usuarioAtualizado.Email != null && usuarioBuscadoEmail == null)
            {
                usuarioBuscado.Email = usuarioAtualizado.Email;
            }

            if (usuarioAtualizado.Senha != null)
            {
                usuarioBuscado.Senha = usuarioAtualizado.Senha;
            }

            if (usuarioAtualizado.IdTipoUsuario != null)
            {
                usuarioBuscado.IdTipoUsuario = usuarioAtualizado.IdTipoUsuario;
            }

            ctx.Usuarios.Update(usuarioBuscado);

            ctx.SaveChanges();
        }

        // MVP - Método de buscar usuários por ID
        public Usuario BuscarPorId(int id)
        {
            return ctx.Usuarios.Include(x => x.Pacientes).Include(x => x.Medicos).FirstOrDefault(x => x.IdUsuario == id);
        }

        // MVP - Método de buscar por email dos usuários para complementar outros métodos
        public Usuario BuscarPorEmail(string email)
        {
            Usuario usuarioBuscado = ctx.Usuarios.FirstOrDefault(x => x.Email == email);

            if (usuarioBuscado != null)
            {
                return usuarioBuscado;
            }

            return null;
        }

        // MVP - Método de cadastrar novos usuários
        public void Cadastrar(Usuario novoUsuario)
        {
            ctx.Usuarios.Add(novoUsuario);

            ctx.SaveChanges();
        }

        // MVP - Método de deletar usuários
        public void Deletar(int id)
        {
            ctx.Usuarios.Remove(BuscarPorId(id));

            ctx.SaveChanges();
        }

        // MVP - Método de listar todos os usuários
        public List<Usuario> Listar()
        {
            return ctx.Usuarios.Include(x => x.Pacientes).Include(x => x.Medicos).ToList();
        }

        // MVP - Método de para criar um token de login para os usuários
        public Usuario Logar(string email, string senha)
        {
            Usuario login = ctx.Usuarios.Include(x => x.IdTipoUsuarioNavigation).FirstOrDefault(x => x.Email == email && x.Senha == senha);

            return login;
        }

        // EXTRA - Método de listar apenas as informações do usuário que está logado
        public List<Usuario> ListarPerfil(int id)
        {
            return ctx.Usuarios
                .Include(x => x.IdTipoUsuarioNavigation)
                .Include(x => x.Medicos)
                .Include(x => x.Pacientes)
                .Where(x => x.IdUsuario == id)
                .ToList();
        }

        // EXTRA - Método de listar todos os usuários sem que suas senhas apareçam
        public List<Usuario> ListarSemSenha()
        {
            var usuarioSemSenha = ctx.Usuarios
            .Include(x => x.IdTipoUsuarioNavigation)
            .Select(x => new Usuario()
            {
                IdUsuario = x.IdUsuario,
                Email = x.Email,
                IdTipoUsuario = x.IdTipoUsuario,
                IdTipoUsuarioNavigation = x.IdTipoUsuarioNavigation
            })
            .Where(x => x.IdTipoUsuarioNavigation.IdTipoUsuario == 2 && x.IdTipoUsuarioNavigation.IdTipoUsuario == 3);

            return usuarioSemSenha.ToList();
        }

        public string BuscarNomePorId(int id)
        {
            Paciente pacienteBuscado = ctx.Pacientes.FirstOrDefault(x => x.IdUsuario == id);

            Medico medicoBuscado = ctx.Medicos.FirstOrDefault(x => x.IdUsuario == id);

            if (pacienteBuscado != null)
            {
                return pacienteBuscado.NomePaciente;
            }

            if (medicoBuscado != null)
            {
                return medicoBuscado.NomeMedico;
            }

            return "Administrador";
        }

        public int BuscarPacientePorId(int id)
        {
            Paciente pacienteBuscado = ctx.Pacientes.FirstOrDefault(x => x.IdUsuario == id);

            if (pacienteBuscado != null)
            {
                return pacienteBuscado.IdPaciente;
            }

            return 0;
        }

        public bool AlterarEmail(int id, Usuario email)
        {
            Usuario usuarioBuscado = ctx.Usuarios.FirstOrDefault(x => x.IdUsuario == id);

            if (usuarioBuscado == null)
            {
                return false;
            }

            if (email.Email != null)
            {
                usuarioBuscado.Email = email.Email;
            }

            ctx.Update(usuarioBuscado);

            ctx.SaveChanges();

            return true;
        }

        public string UploadFoto(IFormFile arquivo, string savingFolder)
        {
            // se a "savingFolder" (pasta de salvamento) está vazia...
            if (savingFolder == null)
            {
                // mescla seu caminho com a pasta "FotosBackUp"
                savingFolder = Path.Combine("FotosBackUp");
            }

            // variavel "pathToSave" (caminho para salvar) será igual a mescla entre o caminho inicial da nossa aplicação + savingFolder
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), savingFolder);

            // caso a pasta estiver com mais que 10 imagens...
            if (savingFolder == "FotosBackUp")
            {
                // será feito uma limpeza para que não tenha problema no desempenho
                // string array "fileEntries" (entradas de arquivo) pega os arquivos do "pathToSave" que é onde está as fotos armazenadas
                string[] fileEntries = Directory.GetFiles(pathToSave);
                // se a quantidade de arquivos for maior ou igual a 10...
                if (fileEntries.Length >= 10)
                {
                    for (int i = 0; i < fileEntries.Length; i++)
                    {
                        // será deletado
                        File.Delete(fileEntries[i]);
                    }
                }
            }

            // se o tamanho do nome do arquivo (da foto, no caso) for maior q 3...
            if (arquivo.FileName.Length > 3)
            {
                // variavel "fileName" que converte o nome do arquivo para uma instância ContentDispositionHeaderValue, ou seja, troca o nome por um id, assim, não irá se repetir arquivos com mesmos nomes na pasta
                var fileName = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');
                // variavel "fullPath" que pega o caminho completo dos arquivos, mesclando o caminho onde é salvo e os nomes dos arquivos
                var fullPath = Path.Combine(pathToSave, fileName);

                // aqui é onde o nome do arquivo é copiado para a pasta certa
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    // onde copia o arquivo
                    arquivo.CopyTo(stream);
                }

                // variavel "nomeArquivo" é igual ao nome do arquivo
                var nomeArquivo = arquivo.FileName;
                // variavel "extensao" é igual ao nome da extensão do arquivo
                string extensao = nomeArquivo.Split('.')[1].Trim();
                // string "nome" é igual a "Guid.NewGuid().ToString()" (esse Guid dará o novo nome para o arquivo, esse nome será um identificador único, um id) + a sua extensão
                string nome = Guid.NewGuid().ToString() + "." + extensao;
                // string "sourceFile" (arquivo de origem) onde será colocado o "modo" de como pesquisar esse arquivo: http://localhost:5000/ + FotosBackUp + / + foto
                string sourceFile = Path.Combine(Directory.GetCurrentDirectory(), savingFolder + "/" + arquivo.FileName);
                // string "source" que é igual: http://localhost:5000/ + FotosBackUp + /
                string source = Path.Combine(Directory.GetCurrentDirectory(), savingFolder + "/");

                // classe FileInfo (que consegue mover, excluir, renomear, etc)
                FileInfo fotoInfo = new FileInfo(sourceFile);
                // após ter preparado todo o arquivo uploaded, ele é movido para a pasta com o seu nome alterado
                fotoInfo.MoveTo(source + nome);
                return nome;
            }
            // caso contrário...
            else
            {
                // retorna null
                return null;
            }
        }

        public string AlterarFoto(IFormFile novaFoto, int idUsuario)
        {
            // verifica se existe um usuário com o tal id
            Usuario usuarioBuscado = ctx.Usuarios.Find(idUsuario);
            // string "fotoAntiga" é igual a foto do usuário (por padrão, "user.png", mas vai sendo alterada)
            string fotoAntiga = usuarioBuscado.Foto;
            // variavel "uparFoto" é igual ao método de de UploadFoto, que coloca a "novaFoto"(arquivo) + "FotoPerfil"(savingFolder)
            var uparFoto = UploadFoto(novaFoto, "FotosPerfil");
            // variavel "pathToSave" é igual: http://localhost:5000/ + FotosPerfil/
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "FotosPerfil/");

            // caso não tenha nenhuma nova foto...
            if (uparFoto == null)
            {
                // retorna null
                return null;
            }
            // caso contrário...
            else
            {
                // foto do usuário buscado será igual a variavel "uparFoto"
                usuarioBuscado.Foto = uparFoto;
                // atualiza o usuário buscado com a nova foto
                ctx.Update(usuarioBuscado);
                // salva as alterações
                ctx.SaveChanges();
            }

            // caso a "fotoAntiga" seja diferente de "user.png"...
            if (fotoAntiga != "user.png")
            {
                // string "filePath" será igual: http://localhost:5000/ + FotosPerfil/ + "user.png"
                string filePath = pathToSave + fotoAntiga;
                // e esse caminho ("filePath") será excluído
                File.Delete(filePath);
            }

            // assim, retorna a foto upada
            return uparFoto;
        }
        
    }
}
