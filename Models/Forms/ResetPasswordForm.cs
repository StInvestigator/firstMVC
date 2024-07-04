using System.ComponentModel.DataAnnotations;

namespace firstMVC.Models.Forms
{
    public class ResetPasswordForm
    {
        [Required(ErrorMessage = "Пароль обов'язковий")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю обов'язкове")]
        [Display(Name = "Підтвердження паролю")]
        public string ConfirmPassword { get; set; }
    }
}
