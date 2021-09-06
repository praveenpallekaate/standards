using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SendGrid.Email
{
    /// <summary>
    /// Email model
    /// </summary>
    public class EmailModel
    {
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] ToAddress { get; set; }
        public string[] CcAddress { get; set; }
        public IEnumerable<AttachmentDetails> Attachments { get; set; }
        public BodyType BodyType { get; set; }
        public string[] Categories { get; set; }
    }

    /// <summary>
    /// Attachment details model
    /// </summary>
    public class AttachmentDetails
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
