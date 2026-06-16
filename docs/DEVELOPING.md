
# Developing for Flood Online Reporting Tool - Report Status

Thank you for your interest in contributing to the Flood Online Reporting Tool - Report Status! This guide will help you set up your development environment and understand the basic components of the project.

## Dependencies

To run this project with minimal modification, you will need the following:

- **PostgreSQL 13+ with PostGIS extension**: This is the default database provider. The project uses Entity Framework, making it adaptable to other providers with minimal effort.
- **.NET 10**: Ensure you have the .NET 10 SDK installed.
- **Message system (optional)**: The project includes a messaging system using Azure Service Bus. You can enable or disable this system through the `Messaging:Enabled` setting in your configuration.

## Getting Started

1. **Clone the repository**:
   ```shell
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Create a new branch**:
   ```shell
   git checkout -b <branch-name>
   ```

3. **AI standards module**:
    
   If you are using GitHub Copilot you can choose to import the Dorset Council UK instruction files. 
   This ensures that code suggestions are more likely to follow the coding standards we are trying to follow at Dorset Council.
   
   To update the standards module, navigate to the `FloodOnlineReportingTool.ReportStatus` solution folder and run:
   ```shell
   git submodule update
   ```
   You may need to initialize the submodule the first time you load it after cloning from the source repository. Use the init flag as follows:
   ```shell
   git submodule update --init 
   ```
   Should you need to switch the submodule to a different branch, navigate to the `FloodOnlineReportingTool.ReportStatus` solution folder and run:
   ```shell
   git submodule set-branch --branch {branch-name} .github/instructions

   git submodule update --remote .github/instructions
   ```

4. **Set up your secrets**:

   Configure the user secrets file for development. See the "User Secrets and Configuration" section below for details.

5. **Run migrations and seed the database**:

   The database schema and seed data are handled by the data project.
   Navigate to the solution folder and run the following command:
   ```shell
   dotnet ef database update --project "FloodOnlineReportingTool.ReportStatus.Database" --startup-project "FloodOnlineReportingTool.ReportStatus"
   ```
   or if using the Package Manager Console in Visual Studio, run:
   ```shell
   Update-Database -Context ApplicationDbContext -StartupProject FloodOnlineReportingTool.ReportStatus
   ```

## Database Setup

This project uses PostgreSQL as its default database. Migrations and data seeding are automated, so no manual database setup is required.

### User
Create a default user with the following credentials:
- **Username**: `fortreportstatus`
- **Password**: Any password you choose
- **Permissions**: can log in, can create databases

The username and password is then used in the connection string FloodReportingReportStatus.

### Connection Strings
The project requires a `ConnectionStrings` section in your user secrets. And a connection string named `FloodReportingReportStatus`. Ensure the connection string contains `;Search Path=fortreportstatus` for proper functionality. See `Example secrets file` below for a more an example.

## User Secrets and Configuration

You must set up user secrets for development. Run the following command in the terminal within the project folder:
- Using Visual Studio:

  Right click the ReportStatus and Messaging projects and select "Manage User Secrets".
- Using .NET CLI:
  ```shell
  dotnet user-secrets init
  ```
Then add your configuration:

- **Messaging**: Used for the optional messaging system.
- **ConnectionStrings**: Required for database access.

Example secrets file:
```json
{
  "ConnectionStrings": {
    "FloodReportingReportStatus": "Host=localhost;Port=5432;Database=YourDatabaseName;Username=YourUserName;Password=YourPassword;SearchPath=fortreportstatus,public"
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "SomeDomain",
    "TenantId": "SomeTenantId",
    "ClientId": "SomeClientId"
  },
  "GovNotify": {
    "ApiKey": "YourApiKey",
    "Templates": {
      "Vulnerable": "YourTemplateId"
    }
  }
}
```

Example Messaging project secrets file:
```json
{
  "ConnectionStrings": {
    "FloodReportingReportStatus": "Host=localhost;Port=5432;Database=YourDatabaseName;Username=YourUserName;Password=YourPassword;SearchPath=fortreportstatus,public",
    "service-bus": "Endpoint=sb://your-namespace.servicebus.windows.net/;SharedAccessKeyName=your-shared-access-key-name;SharedAccessKey=your-shared-access-key"
  },
  "Messaging:TopicSuffix": "-name",
  "Inbox:SubscriptionName": "subscription-name"
}
```

## Authentication

Authentication is required to create a report or duplicate report. The project uses standard .NET Identity authentication.

## Other Customizations

For messaging, the `Messaging` section allows you to enable or disable the MassTransit messaging system. By default, messaging is disabled (`Messaging:Enabled = false`).

## Further Help

For further help, please reach out to the maintainers or consult the documentation. 
