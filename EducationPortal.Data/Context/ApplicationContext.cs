using EducationPortal.Domain.Models;
using EducationPortal.Domain.Models.Materials;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EducationPortal.Data.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
             : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Pdf> Pdfs { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CourseState> CourseStates { get; set; }

        public DbSet<MaterialState> MaterialStates { get; set; }

        public DbSet<MaterialData> MaterilaDatas { get; set; }
    }
}
