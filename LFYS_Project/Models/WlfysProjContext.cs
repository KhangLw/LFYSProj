using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LFYS_Project.Models;

public partial class WlfysProjContext : DbContext
{
    public WlfysProjContext()
    {
    }

    public WlfysProjContext(DbContextOptions<WlfysProjContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryExercise> CategoryExercises { get; set; }

    public virtual DbSet<CategoryOfExercise> CategoryOfExercises { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<FileDocument> FileDocuments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ResultTable> ResultTables { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<UserBadge> UserBadges { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HOAMY;Initial Catalog=WLFYS_Proj;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Avatar).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Facebook).HasMaxLength(200);
            entity.Property(e => e.Instagram).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.Youtube).HasMaxLength(200);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.BadgeId).HasName("PK__Badge__E7989656C98C98C9");

            entity.ToTable("Badge");

            entity.Property(e => e.BadgeId).HasColumnName("badge_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B400930229");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<CategoryExercise>(entity =>
        {
            entity.HasKey(e => e.CeId).HasName("PK__Category__DD9025CDF70A9464");

            entity.ToTable("CategoryExercise");

            entity.Property(e => e.CeId).HasColumnName("ce_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryExercises)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__CategoryE__categ__571DF1D5");

            entity.HasOne(d => d.Exercise).WithMany(p => p.CategoryExercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__CategoryE__exerc__5812160E");
        });

        modelBuilder.Entity<CategoryOfExercise>(entity =>
        {
            entity.HasKey(e => e.CoeId).HasName("PK__Category__93E65D0044CFAA90");

            entity.ToTable("CategoryOfExercise");

            entity.Property(e => e.CoeId).HasColumnName("coe_id");
            entity.Property(e => e.CoeName)
                .HasMaxLength(50)
                .HasColumnName("coe_name");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__8F1EF7AE49CE0848");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Avt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("avt");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .HasColumnName("course_name");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.IsFree).HasColumnName("is_free");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Course__category__4222D4EF");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__9666E8AC78946A6B");

            entity.ToTable("Document");

            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Document__catego__4BAC3F29");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__C121418E58B4753A");

            entity.ToTable("Exercise");

            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Ac).HasColumnName("ac");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<FileDocument>(entity =>
        {
            entity.HasKey(e => e.FiledocId).HasName("PK__FileDocu__6070B1A80322C209");

            entity.ToTable("FileDocument");

            entity.Property(e => e.FiledocId).HasColumnName("filedoc_id");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.FileContent)
                .HasColumnType("ntext")
                .HasColumnName("file_content");
            entity.Property(e => e.FileTitle)
                .HasMaxLength(255)
                .HasColumnName("file_title");

            entity.HasOne(d => d.Document).WithMany(p => p.FileDocuments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__FileDocum__docum__4E88ABD4");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842F2C845D83");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<ResultTable>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__ResultTa__AFB3C3160F1F9ACE");

            entity.ToTable("ResultTable");

            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.Complete).HasColumnName("complete");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Exercise).WithMany(p => p.ResultTables)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__ResultTab__user___02FC7413");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Test__F3FF1C0280255E74");

            entity.ToTable("Test");

            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Intput)
                .HasColumnType("ntext")
                .HasColumnName("intput");
            entity.Property(e => e.Output)
                .HasColumnType("ntext")
                .HasColumnName("output");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("FK__Test__output__6FE99F9F");
        });

        modelBuilder.Entity<UserBadge>(entity =>
        {
            entity.HasKey(e => e.UserbadgeId).HasName("PK__UserBadg__B0DE4084C54AC5CA");

            entity.ToTable("UserBadge");

            entity.Property(e => e.UserbadgeId).HasColumnName("userbadge_id");
            entity.Property(e => e.AwardedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("awarded_at");
            entity.Property(e => e.BadgeId).HasColumnName("badge_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Badge).WithMany(p => p.UserBadges)
                .HasForeignKey(d => d.BadgeId)
                .HasConstraintName("FK__UserBadge__badge__3D5E1FD2");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Video__E8F11E10C1AA0601");

            entity.ToTable("Video");

            entity.Property(e => e.VideoId).HasColumnName("video_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Course).WithMany(p => p.Videos)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Video__course_id__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
