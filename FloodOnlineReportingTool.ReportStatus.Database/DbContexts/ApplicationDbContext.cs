using FloodOnlineReportingTool.ReportStatus.Database.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<FloodReport> FloodReports { get; set; }
    public virtual DbSet<FloodReportSource> FloodReportSources { get; set; }
    public virtual DbSet<FloodReportSourceDuplicate> FloodReportSourceDuplicates { get; set; }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.ReportStatus);

        // Add the inbox and outbox pattern messaging tables
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();

        // Entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}