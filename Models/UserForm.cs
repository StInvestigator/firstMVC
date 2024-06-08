using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class UserForm
    {
        public UserForm() 
        {
            Gallery = new List<IFormFile>();
        }
        public UserForm(User user) 
        {
            Id = user.Id;
            Name = user.Name;
            Age = user.Age;
            IsMale = user.IsMale;
            Balance = user.Balance;
            Birthday = user.Birthday;
            ProfessionId = user.ProfessionId;
        }
        public int Id { get; set; }

        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        public string Name { get; set; }

        [DisplayName("Вік")]
        [Required(ErrorMessage = "Вік обов'язковий")]

        public int Age { get; set; }

        [DisplayName("Чи чоловік")]
        public bool IsMale { get; set; }


        [DisplayName("Баланс")]
        [Required(ErrorMessage = "Баланс обов'язковий")]
        public decimal Balance { get; set; }

        [DisplayName("День Народження")]
        [Required(ErrorMessage = "День Народження обов'язковий")]

        public DateTime Birthday { get; set; }

        [DisplayName("Професія")]

        public int ProfessionId { get; set; }

        [DisplayName("Аватарка")]
        public IFormFile? Image { get; set; }

        [DisplayName("Мої зображення")]
        public List<IFormFile> Gallery { get; set; }
    }
}
