using System.ComponentModel.DataAnnotations;

namespace firstMVC.Models.Forms
{
    public class RegisterForm : LoginForm
    {
        [Required(ErrorMessage = "Паролі повинні співпадати")]
        [Display(Name = "Підтвердження паролю")]
        public string ConfitmPassword { get; set; }
    }
}
