using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace firstMVC.Models
{
    public class Customer : IdentityUser<int>
    {
        public Customer() 
        {
            Users = new HashSet<User>();
        }
        [MaxLength(100)]
        public string? FullName {  get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
