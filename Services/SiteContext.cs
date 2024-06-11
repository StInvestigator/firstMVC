using firstMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace firstMVC.Services
{
    public class SiteContext : DbContext
    {
        public SiteContext(DbContextOptions<SiteContext> options) : base(options) { }

        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        public async Task AddOrEditUser(User newUser)
        {
            try
            {
                var model = await Users.FirstAsync(s => s.Id == newUser.Id);
                model.Profession = newUser.Profession;
                model.Birthday = newUser.Birthday;
                model.Age = newUser.Age;
                model.Name = newUser.Name;
                model.IsMale = newUser.IsMale;
                model.Balance = newUser.Balance;
                model.Gallery = newUser.Gallery;
                model.Image = newUser.Image;
            }
            catch 
            {
                newUser.Skills = new List<UserSkill>();
                Users.Add(newUser);
            }
            await SaveChangesAsync();
        }

        public async Task AddOrEditUser(UserForm form, Image? img, List<Image>? gallery)
        {
            await AddOrEditUser(new User
            {
                Id = form.Id,
                Name = form.Name,
                Age = form.Age,
                Balance = form.Balance,
                Birthday = form.Birthday,
                IsMale = form.IsMale,
                Profession = form.ProfessionId == -1? null : await Professions.FirstAsync(pr=>pr.Id==form.ProfessionId),
                Image = img,
                Gallery = gallery
            });
        }

    }
}
