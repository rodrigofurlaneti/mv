using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Core.Extensions
{
    public static class Mail
    {
        public static void SendMail(string to, string subject, string body, string from, Stream file, string filename, List<string> bcc)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage { IsBodyHtml = true };
            message.Attachments.Add(new Attachment(file, filename));
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();

            foreach (var bc in bcc)
            {
                message.Bcc.Add(bc);
            }

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (var recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    var statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode != SmtpStatusCode.MailboxBusy) &&
                        (statusCode != SmtpStatusCode.MailboxUnavailable)) continue;
                    Thread.Sleep(5000);
                    smtpClient.Send(message);
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                var msg = smtpExc.Message;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static void SendMail(string to, string subject, string body, string from, Stream file, string filename)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage { IsBodyHtml = true };
            message.Attachments.Add(new Attachment(file, filename));
            message.From = new MailAddress(from.Trim());
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (var recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    var statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode != SmtpStatusCode.MailboxBusy) &&
                        (statusCode != SmtpStatusCode.MailboxUnavailable)) continue;
                    Thread.Sleep(5000);
                    smtpClient.Send(message);
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                var msg = smtpExc.Message;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static void SendMail(string to, List<string> cc, string subject, string body, string from, Stream file, string filename)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage
            {
                IsBodyHtml = true,
                Attachments = { new Attachment(file, filename) },
                To = { to.Trim() },
                From = new MailAddress(from.Trim()),
                Subject = subject.Trim(),
                Body = body.Trim()
            };

            foreach (var item in cc)
            {
                message.CC.Add(new MailAddress(item.Trim()));
            }

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (var recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    var statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    switch (statusCode)
                    {
                        case SmtpStatusCode.MailboxBusy:
                        case SmtpStatusCode.MailboxUnavailable:
                            Thread.Sleep(5000);
                            smtpClient.Send(message);
                            break;
                    }
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                var msg = smtpExc.Message;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static void SendMail(string to, string subject, string body, string from)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage { From = new MailAddress(@from.Trim()) };
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (var recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    var statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode != SmtpStatusCode.MailboxBusy) &&
                        (statusCode != SmtpStatusCode.MailboxUnavailable)) continue;
                    Thread.Sleep(5000);
                    smtpClient.Send(message);
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                var msg = smtpExc.Message;
	            throw;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
	            throw;
			}
        }

        public static void SendMail(string to, string subject, string body, string from, List<string> bcc)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage { From = new MailAddress(@from.Trim()) };
            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            foreach (var bc in bcc)
            {
                message.Bcc.Add(bc);
            }
            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static void SendMail(string to, IEnumerable<string> cc, string subject, string body, string from, List<string> cco = null)
        {
            //Create message object and populate with the data from form
            var message = new MailMessage { From = new MailAddress(@from.Trim()) };

            foreach (var item in cc)
            {
                message.CC.Add(new MailAddress(item.Trim()));
            }

            if (cco != null)
            {
                foreach (var email in cco)
                {
                    message.Bcc.Add(email);
                }
            }

            message.To.Add(to.Trim());
            message.Subject = subject.Trim();
            message.Body = body.Trim();
            message.IsBodyHtml = true;

            //Setup SmtpClient to send email. Uses web.config settings.
            var smtpClient = new SmtpClient();


            //Error handling for sending message
            try
            {
                smtpClient.Send(message);
                //Exception contains information on each failed receipient
            }
            catch (SmtpFailedRecipientsException recExc)
            {
                for (var recipient = 0; recipient < recExc.InnerExceptions.Length - 1; recipient++)
                {
                    var statusCode = recExc.InnerExceptions[recipient].StatusCode;

                    if ((statusCode != SmtpStatusCode.MailboxBusy) &&
                        (statusCode != SmtpStatusCode.MailboxUnavailable)) continue;
                    Thread.Sleep(5000);
                    smtpClient.Send(message);
                }
            }
            //General SMTP execptions
            catch (SmtpException smtpExc)
            {
                var msg = smtpExc.Message;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static void SendMail(string host, string user, string pass, string from, string to, IEnumerable<string> cco, string titulo, string body, bool IsHtml)
        {
            var smtpServer = new SmtpClient(host, 587) { Credentials = new NetworkCredential(user, pass) };

            var mailMessage = new MailMessage { From = new MailAddress(@from) };

            mailMessage.To.Add(to);
            foreach (var email in cco)
            {
                mailMessage.Bcc.Add(email);
            }
            mailMessage.Subject = titulo;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = IsHtml;
            smtpServer.Send(mailMessage);
        }

        public static void SendMail(string host, string user, string pass, string from, string to, string titulo, string body, bool IsHtml)
        {
            var smtpServer = new SmtpClient(host, 587) { Credentials = new NetworkCredential(user, pass) };

            var mailMessage = new MailMessage { From = new MailAddress(@from) };

            mailMessage.To.Add(to);
            mailMessage.Subject = titulo;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = IsHtml;
            smtpServer.Send(mailMessage);
        }
    }
}

