using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }
        public decimal Balance { get; set; }
        public DateTime Birthday { get; set; }
        public Profession? Profession { get; set; }
        [DisplayName("Навички")]
        public List<UserSkill>? Skills { get; set; }
        public Image? Image { get; set; }
        public List<Image>? Gallery { get; set; }
    }
}
