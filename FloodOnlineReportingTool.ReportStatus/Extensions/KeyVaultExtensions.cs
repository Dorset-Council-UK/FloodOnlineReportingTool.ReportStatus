using Azure.Identity;
using FloodOnlineReportingTool.ReportStatus.Options;

namespace Microsoft.Extensions.Hosting;

internal static class KeyVaultExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        internal IHostApplicationBuilder AddKeyVaults()
        {
            var keyVaultOptions = builder.Configuration
                .GetSection(KeyVaultOptions.SectionName)
                .Get<KeyVaultOptions>();

            if (keyVaultOptions is null || string.IsNullOrWhiteSpace(keyVaultOptions.VaultName))
            {
                return builder;
            }

            builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{keyVaultOptions.SharedVaultName}.vault.azure.net/"),
                new DefaultAzureCredential());

            if (keyVaultOptions is null || string.IsNullOrWhiteSpace(keyVaultOptions.VaultName))
            {
                return builder;
            }

            builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{keyVaultOptions.VaultName}.vault.azure.net/"),
                new DefaultAzureCredential());

            return builder;
        }
    }
}
