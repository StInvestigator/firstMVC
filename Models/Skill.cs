using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image? Image { get; set; }
    }
}
