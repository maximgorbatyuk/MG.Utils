using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MG.Utils.Abstract;
using MG.Utils.AspNetCore.I18N.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace MG.Utils.AspNetCore.I18N
{
    public static class LocalizationConfig
    {
        public static IMvcBuilder AddCustomDataAnnotationsLocalization(this IMvcBuilder builder)
        {
            return builder
                .AddDataAnnotationsLocalization(
                    options =>
                    {
                        options.DataAnnotationLocalizerProvider = (t, f) => f.Create(null);
                    });
        }

        public static IServiceCollection AddI18N(
            this IServiceCollection services, IList<CultureInfo> supportedCultures)
        {
            supportedCultures.ThrowIfNullOrEmpty(nameof(supportedCultures));

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
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;
            });

            return services;
        }
    }
}