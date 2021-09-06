using Microsoft.Graph;
using System.Linq;
using System.Threading.Tasks;

namespace Core.GraphAPI.Email
{
    /// <summary>
    /// Email service
    /// </summary>
    public class EmailService
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly UserService _userService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="graphServiceClient"></param>
        public EmailService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
            _userService = new UserService(_graphServiceClient);
        }

        /// <summary>
        /// Send email on behalf of a user
        /// </summary>
        /// <param name="emailModel">Email details</param>
        /// <returns>Awaitable task</returns>
        public async Task SendMailOnBehalfAsync(EmailModel emailModel)
        {
            // Get user details of from email
            var users = await _userService
                .GetUsersForEmailAsync(emailModel?.FromEmail);

            if (
                users.Any() &&
                emailModel != null &&
                emailModel.ToAddress != null &&
                emailModel.ToAddress.Any() &&
                !string.IsNullOrEmpty(emailModel.Subject) &&
                !string.IsNullOrEmpty(emailModel.Body)
            )
            {
                var user = users.FirstOrDefault();
                var message = BuildMessage(emailModel);

                // Send mail
                await _graphServiceClient
                    .Users[user.User.Id]
                    .SendMail(message, emailModel.SaveToSentItems)
                    .Request()
                    .PostAsync();
            }
        }

        /// <summary>
        /// Send email from current user
        /// </summary>
        /// <param name="emailModel">Email details</param>
        /// <returns>Awaitable task</returns>
        public async Task SendMailAsync(EmailModel emailModel)
        {
            if (
                emailModel.ToAddress.Any() &&
                !string.IsNullOrEmpty(emailModel.Subject) &&
                !string.IsNullOrEmpty(emailModel.Body)
            )
            {
                var message = BuildMessage(emailModel);

                await _graphServiceClient
                    .Me
                    .SendMail(message, emailModel.SaveToSentItems)
                    .Request()
                    .PostAsync();
            }
        }

        /// <summary>
        /// Build message
        /// </summary>
        /// <param name="emailModel">Email details</param>
        /// <returns>Message details to be sent</returns>
        private Message BuildMessage(EmailModel emailModel)
        {
            var toRecipients = emailModel
                .ToAddress
                .Select(i => new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = i
                    }
                })
                .ToList();

            var ccRecipients = emailModel?
                .CcAddress?
                .Select(i => new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = i
                    }
                })?
                .ToList();

            var attachments = new MessageAttachmentsCollectionPage();

            if (emailModel.Attachments != null && emailModel.Attachments.Any())
            {
                foreach (var attachment in emailModel.Attachments)
                {
                    attachments.Add(new FileAttachment
                    {
                        Name = attachment.FileName,
                        ContentType = attachment.ContentType,
                        ContentBytes = attachment.Content
                    });
                }
            }

            var message = new Message
            {
                Subject = emailModel.Subject,
                Body = new ItemBody
                {
                    ContentType = emailModel.BodyType,
                    Content = emailModel.Body
                },
                ToRecipients = toRecipients
            };

            if (ccRecipients != null && ccRecipients.Any())
            {
                message.CcRecipients = ccRecipients;
            }

            if (emailModel.Attachments != null && emailModel.Attachments.Any())
            {
                message.Attachments = attachments;
            }

            return message;
        }
    }
}
