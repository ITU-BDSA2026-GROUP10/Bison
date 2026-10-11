namespace Bison.Razor.BisonDBContext;
public class BisonDBContext : DbContext
{
    DbSet<Post> Posts {get; set;}
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
        
    }
}