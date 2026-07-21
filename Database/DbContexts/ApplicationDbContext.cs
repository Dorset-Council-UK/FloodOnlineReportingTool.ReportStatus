using FloodOnlineReportingTool.Database.DbContexts;
using FloodOnlineReportingTool.Database.Models;
using FloodOnlineReportingTool.Database.Models.Imports;
using FloodOnlineReportingTool.Database.Models.Messaging;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.Database.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<FloodEvent> FloodEvents { get; set; }
    public virtual DbSet<FloodReport> FloodReports { get; set; }
    public virtual DbSet<FloodReportSource> FloodReportSources { get; set; }
    public virtual DbSet<FloodReportSourceDuplicate> FloodReportSourceDuplicates { get; set; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<FailedImports> FailedImports { get; set; } // Table to track failed imports

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.ReportStatus);

        // Entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}