﻿using System.Net;
using System.Net.Mail;

namespace Sigma.Models
{
    public class Mail
    {
        private static string mailFrom = "sigma_site27@mail.ru";//P4Ty1Myg+oaY
        private static string password = "Jq12J7ycq0mnQtKZUmYd";
        private static MailAddress From = new MailAddress(mailFrom, "Sigma");
        private static SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);

        public Mail()
        {
            smtp.Credentials = new NetworkCredential(mailFrom, password);
            smtp.EnableSsl = true;
            smtp.Timeout = 20000;
        }

        public void SendMessage(string mailTo, string code)
        {
            MailAddress To = new MailAddress(mailTo);

            MailMessage message = new MailMessage(From, To);
            message.Subject = "Email confirmation";
            message.Body = "<div> <div style='padding: 15.0pt 18.75pt 22.5pt 18.75pt; '> <p style='color: #000000;font-size: 15.0pt;font-family: Open Sans, Open Sans, Helvetica, Arial, sans-serif;line-height: 14.25pt;margin: 0 0 0 0;padding: 3.25pt 0 3.25pt 0;'><b>Уважаемый пользователь!</b></p><p style='color: #000000;font-size: 11.5pt;font-family: Open Sans, Open Sans, Helvetica, Arial, sans-serif;line-height: 14.25pt;margin: 0 0 0 0;padding: 11.25pt 0 0 0;'>От&nbsp;вашего имени подана заявка на&nbsp;регистрацию учетной записи.</p><p style='color: #000000;font-size: 11.5pt;font-family: Open Sans, Open Sans, Helvetica, Arial, sans-serif;line-height: 14.25pt;margin: 0 0 0 0;padding: 11.25pt 0 0 0;'>Для завершения регистрации вам необходимо подтвердить адрес электронной почты.</p><p style='color: #000000;font-size: 11.5pt;font-family: Open Sans, Open Sans, Helvetica, Arial, sans-serif;line-height: 14.25pt;margin: 0 0 0 0;padding: 11.25pt 0 0 0;'>Для этого введите в форме регистрации код подтверждения <b>" + code + "</b>.</p><p style='color: #000000;font-size: 11.5pt;font-family: Open Sans, Open Sans, Helvetica, Arial, sans-serif;line-height: 14.25pt;margin: 0 0 0 0;padding: 11.25pt 0 0 0;'>Это письмо отправлено автоматически, пожалуйста, не отвечайте на него.<br></p></div></div>";
            message.IsBodyHtml = true;

            smtp.Send(message);
        }
    }
}