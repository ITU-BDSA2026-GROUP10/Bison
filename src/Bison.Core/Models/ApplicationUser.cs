using Microsoft.AspNetCore.Identity;

namespace Bison.Core.Models;
public class ApplicationUser : IdentityUser
{

}

/* From p. 581 (chapter 23.4.2)
public class AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        
    
    public DbSet<Recipe> Recipes { get; set; }
    }*/