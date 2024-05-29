using System.ComponentModel.DataAnnotations;

namespace UserRegistration1.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Must Be Enter User Name")]
        public string ? userName { get; set; }

        [Required(ErrorMessage ="Must Be Enter Password")]
        public string ?password { get; set; }

    }
}
