using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.SendGrid.Email
{
    /// <summary>
    /// Email body content type
    /// </summary>
    public enum BodyType
    {
        plaintext,
        html
    }

    /// <summary>
    /// Email service
    /// </summary>
    public class EmailService
    {
        private readonly string _apiKey = string.Empty;
        private readonly SendGridClient _client = null;

        /// <summary>
        /// ctor
        /// </summary>
        public EmailService(string apikey)
        {
            _apiKey = apikey;

            _client = new SendGridClient(apikey);
        }

        /// <summary>
        /// Send email from current user
        /// </summary>
        /// <param name="emailModel">Email details</param>
        /// <returns>Awaitable task</returns>
        public async Task<bool> SendMailAsync(EmailModel emailModel)
        {
            if (
                emailModel.ToAddress.Any() &&
                !string.IsNullOrEmpty(emailModel.Subject) &&
                !string.IsNullOrEmpty(emailModel.Body)
            )
            {
                var message = BuildMessage(emailModel);

                var response = await _client.SendEmailAsync(message);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }

            return false;
        }

        /// <summary>
        /// Build message
        /// </summary>
        /// <param name="emailModel">Email details</param>
        /// <returns>Message details to be sent</returns>
        private SendGridMessage BuildMessage(EmailModel emailModel)
        {
            var toRecipients = emailModel
                .ToAddress
                .Select(i => new EmailAddress
                {
                    Email = i
                })
                .ToList();

            var ccRecipients = emailModel?
                .CcAddress?
                .Select(i => new EmailAddress
                {
                    Email = i
                })?
                .ToList();

            var categories = emailModel?
                .Categories?
                .ToList();

            var from = new EmailAddress { Email = emailModel.FromEmail };

            var attachments = new List<Attachment>();

            if (emailModel.Attachments != null && emailModel.Attachments.Any())
            {
                foreach (var attachment in emailModel.Attachments)
                {
                    attachments.Add(new Attachment
                    {
                        Content = Convert.ToBase64String(attachment.Content),
                        Filename = attachment.FileName,
                        Type = attachment.ContentType
                    });
                }
            }

            var message = new SendGridMessage
            {
                Subject = emailModel.Subject,
                From = from
            };

            message.AddTos(toRecipients);

            if (ccRecipients != null && ccRecipients.Any())
            {
                message.AddCcs(ccRecipients);
            }

            if (emailModel.Attachments != null && emailModel.Attachments.Any())
            {
                message.Attachments = attachments;
            }

            if (emailModel.BodyType == BodyType.html)
            {
                message.HtmlContent = emailModel.Body;
            } else if (emailModel.BodyType == BodyType.plaintext)
            {
                message.PlainTextContent = emailModel.Body;
            }

            if (categories != null && categories.Any())
            {
                message.Categories = categories;
            }

            return message;
        }
    }
}
