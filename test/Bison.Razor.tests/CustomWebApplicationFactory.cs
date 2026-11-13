namespace Bison.Razor.tests;

using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Bison.Razor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DataModel;


//See: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/10.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/CustomWebApplicationFactory.cs
//From: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public FakePostRepository fakePostRepository;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == 
                    typeof(IDbContextOptionsConfiguration<BisonDBContext>));

            services.Remove(dbContextDescriptor);
            //Perhaps checkout asp net book page 903?

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<BisonDBContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<BisonDBContext>();
            SeedTestData(db);
            fakePostRepository = new FakePostRepository ();
            fakePostRepository.SetContext(db);
        });

        builder.UseEnvironment("Development");
    }

    private static void SeedTestData(BisonDBContext db)
    {

        Author author1 = new Author(){AuthorId = 1, Name = "First Name", Email = "thisIsAMail@Mail.com", Posts = new List<Post>()};
        Author author2 = new Author(){AuthorId = 1, Name = "Second Name", Email = "thisMailIsSoReal@RealMail.com", Posts = new List<Post>()};

        Observation observation1 = new Observation(){PostId = 1, Author = author1, Text = "I saw a bird, crazy times", TimeStamp = DateTime.Parse("2025-12-12 07:12:00")};
        Observation observation2 = new Observation(){PostId = 2, Author = author2, Text = "I call this bird Bison", TimeStamp = DateTime.Parse("2001-01-01 07:12:00")};
       /*_context.Authors.Add(author1);
        _context.Authors.Add(author2);
        _context.Observations.Add(observation1);
        _context.Observations.Add(observation2);*/

        db.Observations.AddRange(
            new Observation(){PostId = 1, Author = author1, Text = "I saw a bird, crazy times", TimeStamp = DateTime.Parse("2025-12-12 07:12:00")},
            new Observation(){PostId = 2, Author = author2, Text = "I call this bird Bison", TimeStamp = DateTime.Parse("2001-01-01 07:12:00")}
        );
        db.SaveChanges();
    }
}
