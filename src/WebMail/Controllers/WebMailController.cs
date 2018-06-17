using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebMail.Application.Model;
using WebMail.Core;
using WebMail.Core.Abstractions;
using WebMail.Core.Models;

namespace WebMail.Controllers
{
    [Route("api/[controller]")]
    public class WebMailController : Controller
    {
        private readonly IEmail _email;
        private readonly ISender _sender;

        public WebMailController(IEmail email, ISender sender)
        {
            _email = email;
            _sender = sender;
        }

        // GET: api/WebMail
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var senderProxy = (FailoverSenderProxy)_sender;
            var senders = senderProxy.Senders;

            var senderList = new List<string>();
            foreach (var sender in senders)
            {
                senderList.Add(sender.GetType().ToString());
            }

            return senderList.ToArray();
        }

        // POST: api/WebMail
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] EmailInput emailInput)
        {
            if (!ModelState.IsValid)
                return (IActionResult)BadRequest(ModelState);

            var subjectWithLocalTime = $"{emailInput.Subject} - {DateTime.Now.ToLocalTime().ToString()}";
            var attachment = GetAttachmentFromMessage(emailInput.Body);

            _email.To(emailInput.ToAddresses)
                .Subject(subjectWithLocalTime)
                .Body(emailInput.Body)
                .Attach(attachment);
            if (!string.IsNullOrEmpty(emailInput.CcAddresses))
                _email.CC(emailInput.CcAddresses);
            if (!string.IsNullOrEmpty(emailInput.BccAddresses))
                _email.BCC(emailInput.BccAddresses);

            var response = await _sender.SendAsync(_email);

            return response.Successful ? Ok() : (IActionResult)BadRequest(response);
        }

        private Attachment GetAttachmentFromMessage(string message)
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            sw.WriteLine(message);
            sw.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var attachment = new Attachment()
            {
                Data = stream,
                ContentType = "text/plain",
                Filename = "WebMailAttachment.txt"
            };

            return attachment;
        }
    }
}
