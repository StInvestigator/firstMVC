using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace firstMVC.Models
{
    public class User
    {
        public User()
        {
            Skills = new List<UserSkill>();
            Gallery = new List<Image>();
            Reviews = new List<Review>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }
        public decimal Balance { get; set; }
        public DateTime Birthday { get; set; }
        public Profession? Profession { get; set; }
        [DisplayName("Навички")]
        public List<UserSkill> Skills { get; set; }
        public Image? Image { get; set; }
        public List<Image> Gallery { get; set; }
        public List<Review> Reviews { get; set; }
        public Customer? Creator { get; set; }
        public CurrentStatus CurrentStatus { get; set; }
    }
}
