using System.ComponentModel.DataAnnotations;

namespace firstMVC.Models.Forms
{
    public class ChangePasswordForm
    {
        [Required(ErrorMessage = "Поточний пароль обов'язковий")]
        [Display(Name = "Поточний пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Новий пароль обов'язковий")]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю обов'язкове")]
        [Display(Name = "Підтвердження паролю")]
        public string ConfirmPassword { get; set; }
    }
}
