using FloodOnlineReportingTool.ReportStatus.Database.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<FloodLocation> FloodLocations { get; set; }
    public virtual DbSet<FloodLocationFloodReport> FloodLocationFloodReports { get; set; }

    public virtual DbSet<FloodProperty> FloodProperties { get; set; }
    public virtual DbSet<FloodPropertyFloodReport> FloodPropertyReports { get; set; }

    public virtual DbSet<FloodRoad> FloodRoads { get; set; }
    public virtual DbSet<FloodRoadFloodReport> FloodRoadFloodReports { get; set; }

    public virtual DbSet<FloodReport> FloodReports { get; set; }
    public virtual DbSet<FloodReportSource> FloodReportSources { get; set; }

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