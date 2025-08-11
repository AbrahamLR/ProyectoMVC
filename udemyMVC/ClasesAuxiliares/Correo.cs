using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO; 
namespace udemyMVC.ClasesAuxiliares
{
    public class Correo
    {
        public static int enviarCorreo(string correoDestino,string asunto,string contenido,string rutaError)
        {           
            int res = 0;
            try {

                string correo = ConfigurationManager.AppSettings["correo"];
                string clave = ConfigurationManager.AppSettings["clave"];
                string servidor = ConfigurationManager.AppSettings["servidor"];
                int puerto = int.Parse(ConfigurationManager.AppSettings["puerto"]);

                MailMessage mail = new MailMessage();
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = contenido;
                mail.From = new MailAddress(correo);
                mail.To.Add(new MailAddress(correoDestino));
                //Envio de correo
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Host = servidor;
                smtp.Port = puerto;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(correo, clave);
                smtp.Send(mail);
                res = 1;

            }
            catch (Exception e)
            {
                res = 0;
                File.AppendAllText(rutaError, "" + e.ToString());
            }
            return res;
        }
    }
}