# Copilot Instructions

## Project Guidelines
- When using Add-Migration in the Package Manager Console for this solution (FloodOnlineReportingTool.ReportStatus), the correct command requires both -Context and -StartupProject parameters. Example: Add-Migration MigrationName -Context ApplicationDbContext -StartupProject FloodOnlineReportingTool.ReportStatus. The startup project is required because the DbContext registration lives in the main app project, not the Database project.