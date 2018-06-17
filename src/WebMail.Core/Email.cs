using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using WebMail.Core.Abstractions;
using WebMail.Core.Models;

namespace WebMail.Core
{
    public class Email : IEmail
    {
        private IConfiguration _configuration;

        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
            var fromEmail = _configuration.GetSection("EMAIL_FROM").Value;
            var fromName = _configuration.GetSection("EMAIL_FROM_NAME").Value;

            Data = new EmailData()
            {
                FromAddress = new Address() { EmailAddress = fromEmail, Name = fromName }
            };
        }

        public EmailData Data { get; set; }

        public IEmail SetFrom(string emailAddress, string name = "")
        {
            Data.FromAddress = new Address(emailAddress, name);
            return this;
        }

        public IEmail To(string emailAddress, string name)
        {
            if (emailAddress.Contains(";"))
            {
                //email address has semi-colon, try split
                var nameSplit = name.Split(';');
                var addressSplit = emailAddress.Split(';');
                for (int i = 0; i < addressSplit.Length; i++)
                {
                    var currentName = string.Empty;
                    if ((nameSplit.Length - 1) >= i)
                    {
                        currentName = nameSplit[i];
                    }
                    Data.ToAddresses.Add(new Address(addressSplit[i].Trim(), currentName.Trim()));
                }
            }
            else
            {
                Data.ToAddresses.Add(new Address(emailAddress.Trim(), name.Trim()));
            }
            return this;
        }

        public IEmail To(string emailAddress)
        {
            if (emailAddress.Contains(";"))
            {
                foreach (string address in emailAddress.Split(';'))
                {
                    Data.ToAddresses.Add(new Address(address));
                }
            }
            else
            {
                Data.ToAddresses.Add(new Address(emailAddress));
            }

            return this;
        }

        public IEmail To(IList<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.ToAddresses.Add(address);
            }
            return this;
        }

        public IEmail CC(string emailAddress, string name = "")
        {
            if (emailAddress.Contains(";"))
            {
                foreach (string address in emailAddress.Split(';'))
                {
                    Data.CcAddresses.Add(new Address(address));
                }
            }
            else
            {
                Data.CcAddresses.Add(new Address(emailAddress, name));
            }
            return this;
        }

        public IEmail CC(IList<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.CcAddresses.Add(address);
            }
            return this;
        }

        public IEmail BCC(string emailAddress, string name = "")
        {
            if (emailAddress.Contains(";"))
            {
                foreach (string address in emailAddress.Split(';'))
                {
                    Data.BccAddresses.Add(new Address(address));
                }
            }
            else
            {
                Data.BccAddresses.Add(new Address(emailAddress, name));
            }
            return this;
        }

        public IEmail BCC(IList<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.BccAddresses.Add(address);
            }
            return this;
        }

        public IEmail Subject(string subject)
        {
            Data.Subject = subject;
            return this;
        }

        public IEmail Body(string body, bool isHtml = false)
        {
            Data.Body = body;
            return this;
        }

        public IEmail Attach(Attachment attachment)
        {
            if (!Data.Attachments.Contains(attachment))
            {
                Data.Attachments.Add(attachment);
            }

            return this;
        }

        public IEmail Attach(IList<Attachment> attachments)
        {
            foreach (var attachment in attachments.Where(attachment => !Data.Attachments.Contains(attachment)))
            {
                Data.Attachments.Add(attachment);
            }
            return this;
        }
    }
}
