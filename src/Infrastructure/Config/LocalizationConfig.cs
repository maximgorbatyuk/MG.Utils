using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using WebHost.Infrastructure.I18N;
using WebHost.Infrastructure.I18N.Contracts;

namespace WebHost.Infrastructure.Config
{
    public static class LocalizationConfig
    {
        private static readonly CultureInfo[] _supportedCultures =
        {
            new ("en"),
            new ("ru")
        };

        public static IMvcBuilder AddCustomDataAnnotationsLocalization(this IMvcBuilder builder)
        {
            return builder
                .AddDataAnnotationsLocalization(
                    options =>
                    {
                        options.DataAnnotationLocalizerProvider = (t, f) => f.Create(null);
                    });
        }

        public static IServiceCollection AddI18N(this IServiceCollection services)
        {
            services.AddSingleton<ILocalizationJsonSettings, LocalizationJsonSettings>();
            services.AddTransient<IStringLocalizer, JsonFileLocalizer>();
            services.AddSingleton<IStringLocalizerFactory, JsonFileLocalizeFactory>();

            services
                .AddLocalization(opt =>
                {
                    opt.ResourcesPath = "Resources";
                });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(_supportedCultures.First().Name);
                options.SupportedCultures = _supportedCultures;
                options.SupportedUICultures = _supportedCultures;
                options.FallBackToParentUICultures = true;
            });

            return services;
        }
    }
}