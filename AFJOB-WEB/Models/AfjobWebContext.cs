using System;
using Microsoft.EntityFrameworkCore;

namespace AFJOB_WEB.Models
{
    public partial class AfjobWebContext : DbContext
    {
        public AfjobWebContext()
        {
        }

        public AfjobWebContext(DbContextOptions<AfjobWebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationTable> ApplicationTables { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=mercychris-osaze;Database=AFJOB-WEB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationTable>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.ToTable("ApplicationTable");

                entity.Property(e => e.ApplicationId)
                    .ValueGeneratedNever()
                    .HasColumnName("ApplicationID");
                entity.Property(e => e.AppliedAt).HasColumnType("datetime");
                entity.Property(e => e.JobId).HasColumnName("JobID");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.ToTable("Employer");

                entity.Property(e => e.EmployerId)
                    .ValueGeneratedNever()
                    .HasColumnName("EmployerID");
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyWebsite)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Industry)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Interview>(entity =>
            {
                entity.ToTable("Interview");

                entity.Property(e => e.InterviewId)
                    .ValueGeneratedNever()
                    .HasColumnName("InterviewID");
                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
                entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.JobId)
                    .ValueGeneratedNever()
                    .HasColumnName("JobID");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.EmployerId).HasColumnName("EmployerID");
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Resume>(entity =>
            {
                entity.ToTable("Resume");

                entity.Property(e => e.ResumeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ResumeID");
                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.ParsedData).HasColumnType("text");
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");
                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                // Modify the UserId to be an identity column
                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()  // Automatically generated by SQL Server
                    .HasColumnName("UserID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
