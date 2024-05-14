using InsigniaServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsigniaServer.Contexts;

public partial class InsigniaDBContext : DbContext
{
    public InsigniaDBContext()
    {
    }

    public InsigniaDBContext(DbContextOptions<InsigniaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AnswerBlob> AnswerBlobs { get; set; }

    public virtual DbSet<AnswersAnswerBlob> AnswersAnswerBlobs { get; set; }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentQuestionPack> AssignmentQuestionPacks { get; set; }

    public virtual DbSet<AssignmentRegistration> AssignmentRegistrations { get; set; }

    public virtual DbSet<AssignmentScore> AssignmentScores { get; set; }

    public virtual DbSet<AuditRecord> AuditRecords { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<ClassroomSubject> ClassroomSubjects { get; set; }

    public virtual DbSet<Preference> Preferences { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionBlob> QuestionBlobs { get; set; }

    public virtual DbSet<QuestionPack> QuestionPacks { get; set; }

    public virtual DbSet<QuestionPackQuestion> QuestionPackQuestions { get; set; }

    public virtual DbSet<QuestionPackRegistration> QuestionPackRegistrations { get; set; }

    public virtual DbSet<QuestionPackResponse> QuestionPackResponses { get; set; }

    public virtual DbSet<QuestionPackScore> QuestionPackScores { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<QuestionsQuestionBlob> QuestionsQuestionBlobs { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectTeacher> SubjectTeachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserLogon> UserLogons { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PRIMARY");

            entity.ToTable("Answers", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AnswerId, "AnswerID_UNIQUE").IsUnique();

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.AnswerText).HasMaxLength(200);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<AnswerBlob>(entity =>
        {
            entity.HasKey(e => e.AnswerBlobId).HasName("PRIMARY");

            entity.ToTable("AnswerBlobs", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AnswerBlobId, "AnswerBlobID_UNIQUE").IsUnique();

            entity.Property(e => e.AnswerBlobId).HasColumnName("AnswerBlobID");
            entity.Property(e => e.AnswerBlob1)
                .HasColumnType("blob")
                .HasColumnName("AnswerBlob");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<AnswersAnswerBlob>(entity =>
        {
            entity.HasKey(e => new { e.AnswerId, e.AnswerBlobId }).HasName("PRIMARY");

            entity.ToTable("AnswersAnswerBlobs", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AnswerBlobId, "AnswersAnswerBlobs.AnswerBlobID_idx");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.AnswerBlobId).HasColumnName("AnswerBlobID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.AnswerBlob).WithMany(p => p.AnswersAnswerBlobs)
                .HasForeignKey(d => d.AnswerBlobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AnswersAnswerBlobs.AnswerBlobID");

            entity.HasOne(d => d.Answer).WithMany(p => p.AnswersAnswerBlobs)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AnswersAnswerBlobs.AnswerID");
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("Assignments", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AssignmentId, "AssignmentID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.SubjectId, "SubjectID_idx");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.AssignmentDescription).HasMaxLength(120);
            entity.Property(e => e.AssignmentName).HasMaxLength(45);
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Subject).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Assignments.SubjectID");
        });

        modelBuilder.Entity<AssignmentQuestionPack>(entity =>
        {
            entity.HasKey(e => new { e.AssignmentId, e.QuestionPackId }).HasName("PRIMARY");

            entity.ToTable("AssignmentQuestionPacks", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionPackId, "AssignmentQuestionPacks.QuestionPackID_idx");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentQuestionPacks)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentQuestionPacks.AssignmentID");

            entity.HasOne(d => d.QuestionPack).WithMany(p => p.AssignmentQuestionPacks)
                .HasForeignKey(d => d.QuestionPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentQuestionPacks.QuestionPackID");
        });

        modelBuilder.Entity<AssignmentRegistration>(entity =>
        {
            entity.HasKey(e => e.AssignmentRegistrationId).HasName("PRIMARY");

            entity.ToTable("AssignmentRegistrations", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AssignmentId, "AssignmentRegistrations.AssignmentID_idx");

            entity.HasIndex(e => e.UserId, "AssignmentRegistrations.UserID_idx");

            entity.HasIndex(e => e.AssignmentRegistrationId, "ResponseID_UNIQUE").IsUnique();

            entity.Property(e => e.AssignmentRegistrationId).HasColumnName("AssignmentRegistrationID");
            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.RegistrationDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.SubmissionDateTime).HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentRegistrations)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentRegistrations.AssignmentID");

            entity.HasOne(d => d.User).WithMany(p => p.AssignmentRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AssignmentRegistrations.UserID");
        });

        modelBuilder.Entity<AssignmentScore>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PRIMARY");

            entity.ToTable("AssignmentScores", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AssignmentId, "AssignmentID_idx");

            entity.HasIndex(e => e.ScoreId, "ScoreID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "UserID_idx");

            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.ScoreDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
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

            entity.ToTable("AuditRecords", "InsigniaDB_Dev");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ActivityTime).HasColumnType("datetime");
            entity.Property(e => e.ActivityDetails).HasMaxLength(120);
            entity.Property(e => e.ActivityType).HasMaxLength(30);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.User).WithMany(p => p.AuditRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AuditRecords.UserID");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PRIMARY");

            entity.ToTable("Classrooms", "InsigniaDB_Dev");

            entity.HasIndex(e => e.ClassroomId, "ClassroomID_UNIQUE").IsUnique();

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.ClassroomDescription).HasMaxLength(120);
            entity.Property(e => e.ClassroomName).HasMaxLength(45);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<ClassroomSubject>(entity =>
        {
            entity.HasKey(e => new { e.ClassroomId, e.SubjectId }).HasName("PRIMARY");

            entity.ToTable("ClassroomSubject", "InsigniaDB_Dev");

            entity.HasIndex(e => e.SubjectId, "ClassroomSubject.SubjectID_idx");

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Classroom).WithMany(p => p.ClassroomSubjects)
                .HasForeignKey(d => d.ClassroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClassroomSubject.ClassroomID");

            entity.HasOne(d => d.Subject).WithMany(p => p.ClassroomSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClassroomSubject.SubjectID");
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Preferences", "InsigniaDB_Dev");

            entity.HasIndex(e => e.UserId, "UserID_UNIQUE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Locale).HasMaxLength(15);
            entity.Property(e => e.ObserveDaylightSaving).HasDefaultValueSql("'1'");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Utcoffset).HasColumnName("UTCOffset");

            entity.HasOne(d => d.User).WithOne(p => p.Preference)
                .HasForeignKey<Preference>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Preferences.UserID");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PRIMARY");

            entity.ToTable("Questions", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionId, "QuestionID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.QuestionTypeId, "Questions.QuestionType_idx");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.QuestionText).HasMaxLength(500);
            entity.Property(e => e.QuestionTitle).HasMaxLength(45);
            entity.Property(e => e.QuestionTypeId).HasColumnName("QuestionTypeID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Questions.QuestionTypeID");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.QuestionAnswerId).HasName("PRIMARY");

            entity.ToTable("QuestionAnswers", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionAnswerId, "QuestionAnswerID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.AnswerId, "QuestionAnswers.AnswerID_idx");

            entity.HasIndex(e => e.QuestionId, "QuestionAnswers.QuestionID_idx");

            entity.Property(e => e.QuestionAnswerId).HasColumnName("QuestionAnswerID");
            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

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

            entity.ToTable("QuestionBlobs", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionBlobId, "QuestionBlobID_UNIQUE").IsUnique();

            entity.Property(e => e.QuestionBlobId).HasColumnName("QuestionBlobID");
            entity.Property(e => e.QuestionBlob1)
                .HasColumnType("blob")
                .HasColumnName("QuestionBlob");
            entity.Property(e => e.Tmestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<QuestionPack>(entity =>
        {
            entity.HasKey(e => e.QuestionPackId).HasName("PRIMARY");

            entity.ToTable("QuestionPacks", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionPackId, "QuestionPackID_UNIQUE").IsUnique();

            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.QuestionPackDescription).HasMaxLength(120);
            entity.Property(e => e.QuestionPackName).HasMaxLength(45);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<QuestionPackQuestion>(entity =>
        {
            entity.HasKey(e => new { e.QuestionPackId, e.QuestionId }).HasName("PRIMARY");

            entity.ToTable("QuestionPackQuestions", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionId, "QuestionPackQuestions.QuestionID_idx");

            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionPackQuestions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackQuestions.QuestionID");

            entity.HasOne(d => d.QuestionPack).WithMany(p => p.QuestionPackQuestions)
                .HasForeignKey(d => d.QuestionPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackQuestions.QuestionPackID");
        });

        modelBuilder.Entity<QuestionPackRegistration>(entity =>
        {
            entity.HasKey(e => e.QuestionPackRegistrationId).HasName("PRIMARY");

            entity.ToTable("QuestionPackRegistrations", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionPackRegistrationId, "QuestionPackRegistrationID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.AssignmentRegistrationId, "QuestionPackRegistrations.AssignmentRegistrationID_idx");

            entity.HasIndex(e => e.QuestionPackId, "QuestionPackRegistrations.QuestionPackID_idx");

            entity.Property(e => e.QuestionPackRegistrationId).HasColumnName("QuestionPackRegistrationID");
            entity.Property(e => e.AssignmentRegistrationId).HasColumnName("AssignmentRegistrationID");
            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.RegistrationDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.SubmissionDateTime).HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.AssignmentRegistration).WithMany(p => p.QuestionPackRegistrations)
                .HasForeignKey(d => d.AssignmentRegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackRegistrations.AssignmentRegistrationID");

            entity.HasOne(d => d.QuestionPack).WithMany(p => p.QuestionPackRegistrations)
                .HasForeignKey(d => d.QuestionPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackRegistrations.QuestionPackID");
        });

        modelBuilder.Entity<QuestionPackResponse>(entity =>
        {
            entity.HasKey(e => new { e.QuestionPackRegistrationId, e.QuestionPackId, e.QuestionId, e.AnswerId }).HasName("PRIMARY");

            entity.ToTable("QuestionPackResponses", "InsigniaDB_Dev");

            entity.HasIndex(e => e.AnswerId, "QuestionPackResponses.AnswerID_idx");

            entity.HasIndex(e => e.QuestionId, "QuestionPackResponses.QuestionID_idx");

            entity.HasIndex(e => e.QuestionPackId, "QuestionPackResponses.QuestionPackID_idx");

            entity.Property(e => e.QuestionPackRegistrationId)
                .HasMaxLength(45)
                .HasColumnName("QuestionPackRegistrationID");
            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.ResponseDateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Answer).WithMany(p => p.QuestionPackResponses)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackResponses.AnswerID");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionPackResponses)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackResponses.QuestionID");

            entity.HasOne(d => d.QuestionPack).WithMany(p => p.QuestionPackResponses)
                .HasForeignKey(d => d.QuestionPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackResponses.QuestionPackID");
        });

        modelBuilder.Entity<QuestionPackScore>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PRIMARY");

            entity.ToTable("QuestionPackScores", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionPackId, "QuestionPackScores.QuestionPackID_idx");

            entity.HasIndex(e => e.UserId, "QuestionPackScores.UserID_idx");

            entity.HasIndex(e => e.ScoreId, "ScoreID_UNIQUE").IsUnique();

            entity.Property(e => e.ScoreId).HasColumnName("ScoreID");
            entity.Property(e => e.QuestionPackId).HasColumnName("QuestionPackID");
            entity.Property(e => e.ScoreDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.QuestionPack).WithMany(p => p.QuestionPackScores)
                .HasForeignKey(d => d.QuestionPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackScores.QuestionPackID");

            entity.HasOne(d => d.User).WithMany(p => p.QuestionPackScores)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionPackScores.UserID");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.QuestionTypeId).HasName("PRIMARY");

            entity.ToTable("QuestionType", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionTypeId, "QuestionTypeID_UNIQUE").IsUnique();

            entity.Property(e => e.QuestionTypeId).HasColumnName("QuestionTypeID");
            entity.Property(e => e.QuestionType1)
                .HasMaxLength(10)
                .HasColumnName("QuestionType");
            entity.Property(e => e.QuestionTypeDescription).HasMaxLength(120);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<QuestionsQuestionBlob>(entity =>
        {
            entity.HasKey(e => new { e.QuestionId, e.QuestionBlobId }).HasName("PRIMARY");

            entity.ToTable("QuestionsQuestionBlobs", "InsigniaDB_Dev");

            entity.HasIndex(e => e.QuestionBlobId, "QuestionsQuestionBlobs.QuestionBlobID_idx");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.QuestionBlobId).HasColumnName("QuestionBlobID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.QuestionBlob).WithMany(p => p.QuestionsQuestionBlobs)
                .HasForeignKey(d => d.QuestionBlobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionsQuestionBlobs.QuestionBlobID");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionsQuestionBlobs)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionsQuestionBlobs.QuestionID");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PRIMARY");

            entity.ToTable("Subjects", "InsigniaDB_Dev");

            entity.HasIndex(e => e.SubjectId, "SubjectID_UNIQUE").IsUnique();

            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.SubjectDescription).HasMaxLength(120);
            entity.Property(e => e.SubjectName).HasMaxLength(45);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<SubjectTeacher>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SubjectId }).HasName("PRIMARY");

            entity.ToTable("SubjectTeacher", "InsigniaDB_Dev");

            entity.HasIndex(e => e.SubjectId, "SubjectID_idx");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Subject).WithMany(p => p.SubjectTeachers)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubjectTeacher.SubjectID");

            entity.HasOne(d => d.User).WithMany(p => p.SubjectTeachers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubjectTeacher.UserID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("Users", "InsigniaDB_Dev");

            entity.HasIndex(e => e.UserId, "UserID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserName, "UserName_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserName, "UserName_idx");

            entity.HasIndex(e => e.UserTypeId, "UserTypeID_idx");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.FamilyName).HasMaxLength(45);
            entity.Property(e => e.FirstName).HasMaxLength(45);
            entity.Property(e => e.IsEmailVerified)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isEmailVerified");
            entity.Property(e => e.IsPhoneVerified).HasDefaultValueSql("'0'");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.RegistrationDate).HasColumnType("date");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.UserName).HasMaxLength(30);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users.UserTypeID");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("UserCredentials", "InsigniaDB_Dev");

            entity.HasIndex(e => e.UserId, "UserID_UNIQUE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.PasswordHash).HasMaxLength(100);
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.User).WithOne(p => p.UserCredential)
                .HasForeignKey<UserCredential>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Passwords.UserID");
        });

        modelBuilder.Entity<UserLogon>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("UserLogon", "InsigniaDB_Dev");

            entity.HasIndex(e => e.UserId, "UserID_UNIQUE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.LastLogonDate).HasColumnType("datetime");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.User).WithOne(p => p.UserLogon)
                .HasForeignKey<UserLogon>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserLogon.UserID");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PRIMARY");

            entity.ToTable("UserType", "InsigniaDB_Dev");

            entity.HasIndex(e => e.UserTypeId, "UserTypeID_UNIQUE").IsUnique();

            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.UserTypeDesc).HasMaxLength(120);
            entity.Property(e => e.UserTypeName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
