using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext  : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=StudentSystem;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=true;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity
                    .Property(x => x.Name)
                    .IsUnicode();

                entity
                    .Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity
                    .Property(c => c.Description)
                    .IsUnicode();
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity
                    .Property(r => r.Name)
                    .IsUnicode();
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity
                    .HasKey(pk => new { pk.StudentId, pk.CourseId });
            });

        }
    }
}