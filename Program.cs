using firstMVC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");


// new
builder.Services.AddDbContext<SiteContext>(op =>
{
    op.UseSqlite(x =>
    {
        op.UseSqlite(builder.Configuration.GetValue<string>("ConnectionStrings:Default"));
    });
});


// Add services to the container.
builder.Services.AddControllersWithViews();

// file service
builder.Services.AddScoped(pr =>
{
    var enviroment = pr.GetRequiredService<IWebHostEnvironment>();
    return new LocalFileService(Path.Combine(enviroment.WebRootPath, "uploads", "img"));
});

// old
//builder.Services.AddSingleton(pr => new UserService(builder.Configuration.GetValue<string>("FileStorage:UsersFilePath")));
//builder.Services.AddSingleton(pr => new ProfessionService(builder.Configuration.GetValue<string>("FileStorage:ProfessionsFilePath")));
//builder.Services.AddSingleton(pr => new SkillService(builder.Configuration.GetValue<string>("FileStorage:SkillsFilePath")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "UserSkill",
    pattern: "{controller}/{action}/{userId}/{skillId}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
