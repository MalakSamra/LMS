using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class LoginViewModel
    {
        [Required, StringLength(15, MinimumLength = 5)]
        public string UserName { get; set; }
        [Required, StringLength(10, MinimumLength = 3), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
