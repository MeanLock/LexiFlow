using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LexiFlow.DAL.Entities;

public partial class LexiFlowDbContext : DbContext
{
    public LexiFlowDbContext()
    {
    }

    public LexiFlowDbContext(DbContextOptions<LexiFlowDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CollocationCardDetail> CollocationCardDetails { get; set; }

    public virtual DbSet<Deck> Decks { get; set; }

    public virtual DbSet<EmailVerification> EmailVerifications { get; set; }

    public virtual DbSet<IdiomCardDetail> IdiomCardDetails { get; set; }

    public virtual DbSet<PhraseCardDetail> PhraseCardDetails { get; set; }

    public virtual DbSet<ReviewRecord> ReviewRecords { get; set; }

    public virtual DbSet<ReviewSession> ReviewSessions { get; set; }

    public virtual DbSet<ReviewState> ReviewStates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VocabularyCardDetail> VocabularyCardDetails { get; set; }

    public virtual DbSet<VocabularyMeaning> VocabularyMeanings { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cards__3214EC0771C1DBD8");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Deck).WithMany(p => p.Cards)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cards_Decks");

            entity.HasOne(d => d.SourceCard).WithMany(p => p.InverseSourceCard).HasConstraintName("FK_Cards_SourceCard");
        });

        modelBuilder.Entity<CollocationCardDetail>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Collocat__55FECDAEAFD5D9C0");

            entity.Property(e => e.CardId).ValueGeneratedNever();

            entity.HasOne(d => d.Card).WithOne(p => p.CollocationCardDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CollocationCardDetails_Cards");
        });

        modelBuilder.Entity<Deck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Decks__3214EC07639FE8F4");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Owner).WithMany(p => p.Decks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Decks_Users");
        });

        modelBuilder.Entity<EmailVerification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailVer__3214EC07198BCA94");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<IdiomCardDetail>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__IdiomCar__55FECDAE4EE78E41");

            entity.Property(e => e.CardId).ValueGeneratedNever();

            entity.HasOne(d => d.Card).WithOne(p => p.IdiomCardDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdiomCardDetails_Cards");
        });

        modelBuilder.Entity<PhraseCardDetail>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__PhraseCa__55FECDAE1997A2A8");

            entity.Property(e => e.CardId).ValueGeneratedNever();

            entity.HasOne(d => d.Card).WithOne(p => p.PhraseCardDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhraseCardDetails_Cards");
        });

        modelBuilder.Entity<ReviewRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReviewRe__3214EC07FE1D8B4B");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Card).WithMany(p => p.ReviewRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewRecords_Cards");

            entity.HasOne(d => d.ReviewSession).WithMany(p => p.ReviewRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewRecords_Session");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewRecords_Users");
        });

        modelBuilder.Entity<ReviewSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReviewSe__3214EC075A1138D2");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Deck).WithMany(p => p.ReviewSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewSessions_Decks");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewSessions_Users");
        });

        modelBuilder.Entity<ReviewState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReviewSt__3214EC070061C31B");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Card).WithMany(p => p.ReviewStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewStates_Cards");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReviewStates_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07502CE34C");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Role).HasDefaultValue(1);
        });

        modelBuilder.Entity<VocabularyCardDetail>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Vocabula__55FECDAE58FF311C");

            entity.Property(e => e.CardId).ValueGeneratedNever();

            entity.HasOne(d => d.Card).WithOne(p => p.VocabularyCardDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VocabularyCardDetails_Cards");
        });

        modelBuilder.Entity<VocabularyMeaning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vocabula__3214EC072E87F10E");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Card).WithMany(p => p.VocabularyMeanings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VocabularyMeanings_Cards");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
