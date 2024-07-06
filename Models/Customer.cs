using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace firstMVC.Models
{
    public class Customer : IdentityUser<int>
    {
        [MaxLength(100)]
        public string? FullName {  get; set; }
    }
}
