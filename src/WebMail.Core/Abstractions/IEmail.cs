using System.Collections.Generic;
using WebMail.Core.Models;

namespace WebMail.Core.Abstractions
{
    public interface IEmail
    {
        EmailData Data { get; set; }

        IEmail SetFrom(string emailAddress, string name);

        IEmail To(string emailAddress, string name);

        IEmail To(string emailAddress);

        IEmail To(IList<Address> mailAddresses);

        IEmail CC(string emailAddress, string name = "");

        IEmail CC(IList<Address> mailAddresses);

        IEmail BCC(string emailAddress, string name = "");

        IEmail BCC(IList<Address> mailAddresses);

        IEmail Subject(string subject);

        IEmail Body(string body, bool isHtml = false);

        IEmail Attach(Attachment attachment);

        IEmail Attach(IList<Attachment> attachments);
    }
}
