namespace Bison.Razor;
using Microsoft.EntityFrameworkCore;
using DataModel;
public class BisonDBContext : DbContext
{
    public DbSet<Post> Posts {get; set;}
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
        
    }
}