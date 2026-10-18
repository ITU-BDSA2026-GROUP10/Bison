namespace Bison.Razor;
using Microsoft.EntityFrameworkCore;
using DataModel;
using System.Net;

public class BisonDBContext : DbContext
{
    public DbSet<Post> Posts {get; set;}
    public DbSet<Author> Authors {get; set;}

    public DbSet<Taxon> Taxons {get; set;}
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
        
    }
}