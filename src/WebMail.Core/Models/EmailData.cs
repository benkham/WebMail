using System.Collections.Generic;

namespace WebMail.Core.Models
{
    public class EmailData
    {
        public List<Address> ToAddresses { get; set; }
        public List<Address> CcAddresses { get; set; }
        public List<Address> BccAddresses { get; set; }
        public List<Attachment> Attachments { get; set; }
        public Address FromAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailData()
        {
            ToAddresses = new List<Address>();
            CcAddresses = new List<Address>();
            BccAddresses = new List<Address>();
            Attachments = new List<Attachment>();
        }
    }
}
