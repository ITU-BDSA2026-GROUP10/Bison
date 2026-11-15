namespace Bison.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bison.Core.Models;
using System.Net;

public class BisonDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Post> Posts {get; set;}
    public DbSet<Observation> Observations {get; set;}
    public DbSet<Comment> Comments {get; set;}
    public DbSet<Proposal> Proposals {get; set;}
    public DbSet<Author> Authors {get; set;}
    public DbSet<Taxon> Taxons {get; set;}
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
        
    }
}