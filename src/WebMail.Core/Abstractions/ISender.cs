using System.Threading;
using System.Threading.Tasks;
using WebMail.Core.Models;

namespace WebMail.Core.Abstractions
{
    public interface ISender
    {
        Task<SendResponse> SendAsync(IEmail email, CancellationToken? token = null);
    }
}
