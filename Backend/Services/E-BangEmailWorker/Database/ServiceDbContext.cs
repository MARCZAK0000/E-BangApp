using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker.Database;

public partial class ServiceDbContext : DbContext
{
    public ServiceDbContext()
    {
    }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<HistoryEmail> HistoryEmails { get; set; }
    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<EmailSetting> EmailSettings { get; set; }

    public virtual DbSet<ProcedureLog> ProcedureLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistoryEmail>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__Emails__7ED91AEF1294F685");

            entity.ToTable("Emails", "History");

            entity.HasIndex(e => e.SentDate, "IDX_SentDate");

            entity.Property(e => e.EmailId).HasColumnName("EmailID");
            entity.Property(e => e.HistoryProcessedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Recipient).HasMaxLength(255);
            entity.Property(e => e.SentDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmailSetting>(entity =>
        {
            entity.ToTable("EmailSettings", "Settings");
        });

        modelBuilder.Entity<ProcedureLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Procedur__3214EC07DDAC8AC2");

            entity.ToTable("ProcedureLog", "Command");

            entity.HasIndex(e => e.ProcedureName, "IX_ProcedureLog_ProcedureName");

            entity.Property(e => e.ExecutionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(4000);
            entity.Property(e => e.ProcedureName).HasMaxLength(128);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
