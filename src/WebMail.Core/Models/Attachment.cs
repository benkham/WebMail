using System.IO;

namespace WebMail.Core.Models
{
    public class Attachment
    {
        public bool IsInline { get; set; }
        public string Filename { get; set; }
        public Stream Data { get; set; }
        public string ContentType { get; set; }
    }
}
