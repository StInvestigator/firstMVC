using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class Profession
    {
        public int Id { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Професія необхідна")]
        public string Name { get; set; }
    }
}
