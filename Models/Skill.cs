using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Назва необхідна")]
        public string Name { get; set; }
    }
}
