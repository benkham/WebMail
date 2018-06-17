using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebMail.Core.Abstractions;
using WebMail.Core.Models;

namespace WebMail.Core
{
    public class FailoverSenderProxy : ISender
    {
        private readonly List<ISender> _senders;

        public FailoverSenderProxy()
        {
            _senders = new List<ISender>();
        }

        public List<ISender> Senders
        {
            get
            {
                return _senders;
            }
        }

        public FailoverSenderProxy RegisterSender(ISender sender)
        {
            _senders.Add(sender);
            return this;
        }

        public Task<SendResponse> SendAsync(IEmail email, CancellationToken? token = null)
        {
            Task<SendResponse> response = null;
            foreach (var sender in _senders)
            {
                try
                {
                    response = sender.SendAsync(email, token);
                    if (!response.Result.Successful)
                        continue;

                    return response;
                }
                catch
                {
                    // Failed, continue to next sender
                    continue;
                }
            }

            // All the senders failed so pass the message
            response.Result.ErrorMessages.Add("Oops! Unable to send email. Please check your email providers.");
            return response;
        }
    }
}
