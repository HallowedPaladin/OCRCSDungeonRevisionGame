using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Entities;

public partial class ApiserverDevContext : DbContext
{
    public ApiserverDevContext()
    {
    }

    public ApiserverDevContext(DbContextOptions<ApiserverDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AnswerBlob> AnswerBlobs { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentScore> AssignmentScores { get; set; }

    public virtual DbSet<AuditRecord> AuditRecords { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<Preference> Preferences { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionBlob> QuestionBlobs { get; set; }

    public virtual DbSet<QuestionPack> QuestionPacks { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogon> UserLogons { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=W3stf13ld;database=APIServerDev");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PRIMARY");

            entity.ToTable("Answers", "APIServerDev");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.AnswerText).HasMaxLength(150);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasMany(d => d.AnswerBlobs).WithMany(p => p.Answers)
                .UsingEntity<Dictionary<string, object>>(
                    "AnswersAnswerBlob",
                    r => r.HasOne<AnswerBlob>().WithMany()
                        .HasForeignKey("AnswerBlobId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AnswersAnswerBlobs.AnswerBlobID"),
                    l => l.HasOne<Answer>().WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AnswersAnswerBlobs.AnswerID"),
                    j =>
                    {
                        j.HasKey("AnswerId", "AnswerBlobId").HasName("PRIMARY");
                        j.ToTable("AnswersAnswerBlobs", "APIServerDev");
                        j.HasIndex(new[] { "AnswerBlobId" }, "AnswersAnswerBlobs.AnswerBlobID_idx");
                        j.IndexerProperty<int>("AnswerId").HasColumnName("AnswerID");
                        j.IndexerProperty<int>("AnswerBlobId").HasColumnName("AnswerBlobID");
                    });
        });

        modelBuilder.Entity<AnswerBlob>(entity =>
        {
            entity.HasKey(e => e.AnswerBlobId).HasName("PRIMARY");

            entity.ToTable("AnswerBlobs", "APIServerDev");

            entity.Property(e => e.AnswerBlobId).HasColumnName("AnswerBlobID");
            entity.Property(e => e.AnswerBlob1)
                .HasColumnType("blob")
                .HasColumnName("AnswerBlob");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("Assignments", "APIServerDev");

            entity.HasIndex(e => e.SubjectId, "SubjectID_idx");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.AssignmentDescription).HasMaxLength(120);
            entity.Property(e => e.AssignmentName).HasMaxLength(45);
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Subject).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Assignments.SubjectID");
        });

        modelBuilder.Entity<AssignmentScore>(entity =>
        {
            entity.HasKey(e => new { e.Date, e.ScoreId }).HasName("PRIMARY");

            entity.ToTable("AssignmentScores", "APIServerDev");

            entity.HasIndex(e => e.AssignmentId, "AssignmentID_idx");

            entity.HasIndex(e => e.UserId, "UserID_idx");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentScores)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentScores.AssignmentID");

            entity.HasOne(d => d.User).WithMany(p => p.AssignmentScores)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentScores.UserID");
        });

        modelBuilder.Entity<AuditRecord>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ActivityTime }).HasName("PRIMARY");

            entity.ToTable("AuditRecords", "APIServerDev");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ActivityTime).HasColumnType("datetime");
            entity.Property(e => e.ActivityDetails).HasMaxLength(120);
            entity.Property(e => e.ActivityType).HasMaxLength(30);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.AuditRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AuditRecords.UserID");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PRIMARY");

            entity.ToTable("Classrooms", "APIServerDev");

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.ClassroomDescription).HasMaxLength(45);
            entity.Property(e => e.ClassroomName).HasMaxLength(45);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasMany(d => d.Subjects).WithMany(p => p.Classrooms)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassroomSubject",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ClassroomSubject.SubjectID"),
                    l => l.HasOne<Classroom>().WithMany()
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ClassroomSubject.ClassroomID"),
                    j =>
                    {
                        j.HasKey("ClassroomId", "SubjectId").HasName("PRIMARY");
                        j.ToTable("ClassroomSubject", "APIServerDev");
                        j.HasIndex(new[] { "SubjectId" }, "ClassroomSubject.SubjectID_idx");
                        j.IndexerProperty<int>("ClassroomId").HasColumnName("ClassroomID");
                        j.IndexerProperty<int>("SubjectId").HasColumnName("SubjectID");
                    });
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Passwords", "APIServerDev");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.Password)
                .HasForeignKey<Password>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Passwords.UserID");
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Preferences", "APIServerDev");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Locale).HasMaxLength(10);

            entity.HasOne(d => d.User).WithOne(p => p.Preference)
                .HasForeignKey<Preference>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Preferences.UserID");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PRIMARY");

            entity.ToTable("Questions", "APIServerDev");

            entity.HasIndex(e => e.QuestionTypeId, "Questions.QuestionType_idx");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.QuestionText).HasMaxLength(500);
            entity.Property(e => e.QuestionTypeId).HasColumnName("QuestionTypeID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Questions.QuestionTypeID");

            entity.HasMany(d => d.QuestionBlobs).WithMany(p => p.Questions)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionsQuestionBlob",
                    r => r.HasOne<QuestionBlob>().WithMany()
                        .HasForeignKey("QuestionBlobId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("QuestionsQuestionBlobs.QuestionBlobID"),
                    l => l.HasOne<Question>().WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("QuestionsQuestionBlobs.QuestionID"),
                    j =>
                    {
                        j.HasKey("QuestionId", "QuestionBlobId").HasName("PRIMARY");
                        j.ToTable("QuestionsQuestionBlobs", "APIServerDev");
                        j.HasIndex(new[] { "QuestionBlobId" }, "QuestionsQuestionBlobs.QuestionBlobID_idx");
                        j.IndexerProperty<int>("QuestionId").HasColumnName("QuestionID");
                        j.IndexerProperty<int>("QuestionBlobId").HasColumnName("QuestionBlobID");
                    });
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.QuestionAnswerId).HasName("PRIMARY");

            entity.ToTable("QuestionAnswers", "APIServerDev");

            entity.HasIndex(e => e.AnswerId, "QuestionAnswers.AnswerID_idx");

            entity.HasIndex(e => e.QuestionId, "QuestionAnswers.QuestionID_idx");

            entity.Property(e => e.QuestionAnswerId).HasColumnName("QuestionAnswerID");
            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Answer).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionAnswers.AnswerID");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionAnswers.QuestionID");
        });

        modelBuilder.Entity<QuestionBlob>(entity =>
        {
            entity.HasKey(e => e.QuestionBlobId).HasName("PRIMARY");

            entity.ToTable("QuestionBlobs", "APIServerDev");

            entity.Property(e => e.QuestionBlobId).HasColumnName("QuestionBlobID");
            entity.Property(e => e.QuestionBlob1)
                .HasColumnType("blob")
                .HasColumnName("QuestionBlob");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuestionPack>(entity =>
        {
            entity.HasKey(e => e.QuestionPackId).HasName("PRIMARY");

            entity.ToTable("QuestionPacks", "APIServerDev");

            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.QuestionPackDescription).HasMaxLength(120);
            entity.Property(e => e.QuestionPackName).HasMaxLength(45);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.QuestionTypeId).HasName("PRIMARY");

            entity.ToTable("QuestionType", "APIServerDev");

            entity.Property(e => e.QuestionTypeId).HasColumnName("QuestionTypeID");
            entity.Property(e => e.QuestionType1)
                .HasMaxLength(10)
                .HasColumnName("QuestionType");
            entity.Property(e => e.QuestionTypeDescription).HasMaxLength(120);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PRIMARY");

            entity.ToTable("Subjects", "APIServerDev");

            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.SubjectDescription).HasMaxLength(120);
            entity.Property(e => e.SubjectName).HasMaxLength(45);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Users", "APIServerDev");

            entity.HasIndex(e => e.UserName, "UserName_idx");

            entity.HasIndex(e => e.UserTypeId, "UserTypeID_idx");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(70);
            entity.Property(e => e.FamilyName).HasMaxLength(45);
            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(30);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users.UserTypeID");

            entity.HasMany(d => d.Subjects).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "SubjectTeacher",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SubjectTeacher.SubjectID"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SubjectTeacher.UserID"),
                    j =>
                    {
                        j.HasKey("UserId", "SubjectId").HasName("PRIMARY");
                        j.ToTable("SubjectTeacher", "APIServerDev");
                        j.HasIndex(new[] { "SubjectId" }, "SubjectID_idx");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("SubjectId").HasColumnName("SubjectID");
                    });
        });

        modelBuilder.Entity<UserLogon>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("UserLogon", "APIServerDev");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LastLogonDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.UserLogon)
                .HasForeignKey<UserLogon>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserLogon.UserID");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PRIMARY");

            entity.ToTable("UserType", "APIServerDev");

            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserTypeDesc).HasMaxLength(120);
            entity.Property(e => e.UserTypeName).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
