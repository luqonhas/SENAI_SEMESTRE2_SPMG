using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using senai.spmg.webAPI.Models;
using senai.spmg.webAPI.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace senai.spmg.webAPI.Services
{
    public class MailService : IMailService
    {
        // para acessar os dados do JSON em tempo de execução
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        // EXTRA - Método para enviar emails manualmente
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            // criação de um objeto de MimeMessage (MimeKit) e envia-lo usando o SMTPClient (MailKit)
            var email = new MimeMessage();

            // cria um novo objeto de MimeMessage e adiciona o Sender, To Address e o Subject nesse objeto
            // seriam os dados relacionado à mensagem (assunto, corpo) do mailRequest e os dados que obtemos do arquivo JSON
            email.Sender = MailboxAddress.Parse(_mailSettings.From);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            // se tiver algum anexo (arquivos) no objeto de solicitação, tranformamos o arquivo em um anexo e o adicionamos à mensagem de email com um objeto de anexo do body builder (classe)
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            // aqui temos a parte de HTML do email da propriedade body da solicitação
            builder.HtmlBody = mailRequest.Body;

            // aqui adiciona o anexo e o corpo HTML ao corpo do email
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.From, _mailSettings.Password);

            // envia a mensagem usando o método SendMailAsync do SMTP
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

        // EXTRA - Método para enviar emails de boas vindas automáticos
        public async Task SendWelcomeEmailAsync(WelcomeRequest request, string emailUser)
        {
            // colocamos o caminho do arquivo onde está o template do HTML de boas vindas
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";

            // vai ler o arquivo em uma string
            StreamReader streamReader = new StreamReader(FilePath);
            string MailText = streamReader.ReadToEnd();
            streamReader.Close();

            request.ToEmail = emailUser;

            // substitui a tag de email para os dados reais (o próprio email)
            MailText = MailText.Replace("[email]", request.ToEmail);

            // prepara o email
            var email = new MimeMessage();
            // coloca quem vai mandar
            email.Sender = MailboxAddress.Parse(_mailSettings.From);
            // adiciona para quem vai mandar
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            // coloca um assunto
            email.Subject = "Bem Vindo(a) à rede SP Medical Group!";

            // adiciona um corpo padrão
            var builder = new BodyBuilder();
            // define o do email a partir do modelo string (que era o HTML)
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();

            // se conecta com o SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.From, _mailSettings.Password);

            // envia o email
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
