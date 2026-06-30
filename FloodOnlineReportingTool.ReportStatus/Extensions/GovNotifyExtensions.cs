using FloodOnlineReportingTool.ReportStatus.Services;
using FloodOnlineReportingTool.ReportStatus.Options;
using Notify.Client;
using Notify.Interfaces;

namespace Microsoft.AspNetCore.Builder;

internal static class GovNotifyExtensions
{
    private const string GovNotifyClientName = "GovNotifyClient";

    public static TBuilder AddGovNotify<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var govNotifyOptions = builder.AddGovNotifyOptions();
        builder.Services
            .AddGovNotifyHttpClient()
            .AddGovNotifyHttpClientWrapper()
            .AddGovNotifyNotificationClient(govNotifyOptions.ApiKey)
            .AddScoped<IGovNotifyEmailSender, GovNotifyEmailSender>();

        return builder;
    }

    private static GovNotifyOptions AddGovNotifyOptions<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var section = builder.Configuration.GetRequiredSection(GovNotifyOptions.SectionName);
        builder.Services.Configure<GovNotifyOptions>(section);
        var options = section.Get<GovNotifyOptions>()
            ?? throw new InvalidOperationException($"There was a problem getting the settings from the {GovNotifyOptions.SectionName} section");
        return options;
    }

    /// <summary>
    /// Setup the GovNotify HttpClient using the HttpClientFactory
    /// </summary>
    /// <remarks>Uses Microsoft's new Http resilience library <see href="https://www.nuget.org/packages/Microsoft.Extensions.Http.Resilience/">Microsoft.Extensions.Http.Resilience</see></remarks>
    private static IServiceCollection AddGovNotifyHttpClient(this IServiceCollection services)
    {
        services
            .AddHttpClient(GovNotifyClientName)
            .AddStandardResilienceHandler();

        return services;
    }

    /// <summary>
    /// Setup the GovNotify HttpClientWrapper using the HttpClientFactory
    /// </summary>
    private static IServiceCollection AddGovNotifyHttpClientWrapper(this IServiceCollection services)
    {
        services.AddTransient<IHttpClient>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(GovNotifyClientName);
            return new HttpClientWrapper(httpClient);
        });

        return services;
    }

    /// <summary>
    /// Setup the GovNotify NotificationClient using the configured HttpClientWrapper
    /// </summary>
    private static IServiceCollection AddGovNotifyNotificationClient(this IServiceCollection services, string apiKey)
    {
        services.AddTransient<NotificationClient>(serviceProvider =>
        {
            var httpClientWrapper = serviceProvider.GetRequiredService<IHttpClient>();
            return new NotificationClient(httpClientWrapper, apiKey);
        });

        return services;
    }
}
