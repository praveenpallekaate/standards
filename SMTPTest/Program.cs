using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace SMTPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting SMTP test");

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            string from = configuration.GetSection("SMTP:from").Value;
            string to = configuration.GetSection("SMTP:to").Value;
            string subject = configuration.GetSection("SMTP:subject").Value;
            string body = configuration.GetSection("SMTP:body").Value;
            string username = configuration.GetSection("SMTP:username").Value;
            string pass = configuration.GetSection("SMTP:pass").Value;
            string host = configuration.GetSection("SMTP:host").Value;
            int port = int.Parse(configuration.GetSection("SMTP:port").Value);

            var msg = new MailMessage(from, to, subject, body);

            msg.IsBodyHtml = true;

            var client = new SmtpClient(host, port);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(username, pass);
            }
            else
            {
                client.UseDefaultCredentials = true;
            }

            client.Send(msg);

            Console.WriteLine($"Mail sent to {to}");
            Console.WriteLine("SMTP test completed");
        }
    }
}
