using System.ComponentModel.DataAnnotations;

namespace WebMail.Application.Model
{
    public class EmailInput
    {
        [Required]
        [MinLength(10)]
        [StringLength(200)]
        public string ToAddresses { get; set; }

        [StringLength(200)]
        public string CcAddresses { get; set; }

        [StringLength(200)]
        public string BccAddresses { get; set; }

        [Required]
        [MinLength(10)]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [MinLength(10)]
        [StringLength(500)]
        public string Body { get; set; }
    }
}
