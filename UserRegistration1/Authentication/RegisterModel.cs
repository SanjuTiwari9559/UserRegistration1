using System.ComponentModel.DataAnnotations;

namespace UserRegistration1.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage="Must Be Enter UserName")]
        public string ?UserName { get; set; }
        [Required(ErrorMessage ="Must be Enter Password")]
        public string ?Password { get; set; }
        [Required(ErrorMessage ="Must Be Enter Gmail")]
        public string? Email { get; set; }

    }
}
