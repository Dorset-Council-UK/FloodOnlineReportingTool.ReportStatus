using FloodOnlineReportingTool.ReportStatus.Database.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<FloodLocation> FloodLocations { get; set; }
    public DbSet<FloodLocationFloodReport> FloodLocationReports { get; set; }

    public DbSet<FloodProperty> FloodProperties { get; set; }
    public DbSet<FloodPropertyFloodReport> FloodPropertyReports { get; set; }

    public DbSet<FloodRoad> FloodRoads { get; set; }
    public DbSet<FloodRoadFloodReport> FloodRoadReports { get; set; }

    public DbSet<FloodReport> FloodReports { get; set; }
    public DbSet<FloodReportSource> FloodReportSources { get; set; }

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