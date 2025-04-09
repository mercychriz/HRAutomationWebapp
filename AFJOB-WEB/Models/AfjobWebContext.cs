using AFJOB_WEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AFJOB_WEB.Models
{
    public class AfjobWebContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AfjobWebContext(DbContextOptions<AfjobWebContext> options)
            : base(options)
        {
        }

        // DbSets for each of your entities
        public virtual DbSet<ApplicationTable> ApplicationTables { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ManpowerPlanning> ManpowerPlannings { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<JobAnalysis> JobAnalyses { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }
        public virtual DbSet<JobDescription> JobDescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Replace this with your actual connection string
                optionsBuilder.UseSqlServer("Server=mercychris-osaze;Database=AFJOB-WEB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call base!
            base.OnModelCreating(modelBuilder);

            // ✅ Employer Configuration
            modelBuilder.Entity<Employer>(entity =>
            {
                entity.ToTable("Employers");

                entity.HasKey(e => e.EmployerId);

                entity.Property(e => e.EmployerId)
                    .ValueGeneratedOnAdd(); // Auto-Increment (IDENTITY)

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .IsRequired();

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.CompanyWebsite)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Industry)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                // Relationship (Optional)
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ✅ Job Configuration
            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Jobs");

                entity.HasKey(j => j.JobId);

                entity.Property(j => j.JobId)
                    .ValueGeneratedOnAdd();

                entity.Property(j => j.EmployerId)
                    .HasMaxLength(450)
                    .IsRequired();

                entity.Property(j => j.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(j => j.Description)
                    .IsRequired();

                entity.Property(j => j.Location)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(j => j.Salary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(j => j.CreatedAt)
                    .HasColumnType("datetime");

                entity.Property(j => j.ExpiryDate)
                    .HasColumnType("datetime");

                // Job belongs to a User (Employer)
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(j => j.EmployerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ✅ JobAnalysis Configuration
            modelBuilder.Entity<JobAnalysis>(entity =>
            {
                entity.HasKey(ja => ja.JobAnalysisId);

                entity.HasOne(ja => ja.Job)
                    .WithOne(j => j.JobAnalysis)
                    .HasForeignKey<JobAnalysis>(ja => ja.JobId);
            });

            // ✅ JobDescription Configuration
            modelBuilder.Entity<JobDescription>(entity =>
            {
                entity.HasKey(jd => jd.JobDescriptionId);

                entity.HasOne(jd => jd.Job)
                    .WithOne(j => j.JobDescription)
                    .HasForeignKey<JobDescription>(jd => jd.JobId);
            });

            // ✅ Resume Configuration
            modelBuilder.Entity<Resume>(entity =>
            {
                entity.HasKey(e => e.ResumeId);

                entity.Property(e => e.ResumeId)
                    .ValueGeneratedNever();

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParsedData)
                    .HasColumnType("text");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450);
            });

            // ✅ Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId);

                entity.Property(r => r.RoleId)
                    .ValueGeneratedNever();

                entity.Property(r => r.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            // ✅ User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(u => u.Id)
                    .HasMaxLength(450)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UserID");

                entity.Property(u => u.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(u => u.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(u => u.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(u => u.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(u => u.CreatedAt)
                    .HasColumnType("datetime");
            });
        }
    }
}
