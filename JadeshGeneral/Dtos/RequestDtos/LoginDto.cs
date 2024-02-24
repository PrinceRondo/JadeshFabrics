using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeshGeneral.Dtos.RequestDtos
{
    public record LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
        public string? NewPassword { get; set; }
        public string? VerificationCode { get; set; }
    }
}
