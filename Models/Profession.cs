using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class Profession
    {

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Професія")]
        public string Name { get; set; }
    }
}
