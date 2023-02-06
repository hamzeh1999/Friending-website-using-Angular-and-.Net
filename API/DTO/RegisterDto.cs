using System.ComponentModel.DataAnnotations;
namespace API.DTO
{
    public class RegisterDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }


        [Required] public string knownAs { get; set; }

        [Required] public string city { get; set; }
        [Required] public string country { get; set; }
        [Required] public string dateOfBirth { get; set; }
        [Required] public string gender { get; set; }

    }
}