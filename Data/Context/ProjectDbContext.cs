using AbidiCompanySenario.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbidiCompanySenario.Data.Context
{
    public class ProjectDbContext: DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
           : base(options) { }

        public DbSet<Personnel> Personnels { get; set; }

    }
}
