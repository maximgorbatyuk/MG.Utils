﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebHost.Infrastructure.Config
{
    public static class SwaggerConfig
    {
        private const string Version = "v1";

        private const string Endpoint = "/swagger/v1/swagger.json";

        public static void ApplyUI(SwaggerUIOptions config, string appName)
        {
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint(Endpoint, appName);
            config.DisplayRequestDuration();
            config.DocExpansion(DocExpansion.List);
        }

        public static void Apply(SwaggerGenOptions config, string appName, string frontendAppLink)
        {
            config.SwaggerDoc(Version, new OpenApiInfo
            {
                Title = appName,
                Version = Version,
                Description = $"Frontend: {frontendAppLink}"
            });

            // copied from https://stackoverflow.com/a/58972781
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });

            // Locate the XML file being generated by ASP.NET...
            var xmlFile = $"{Assembly.GetEntryAssembly() !.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                // ... and tell Swagger to use those XML comments.
                config.IncludeXmlComments(xmlPath);
            }

            config.OperationFilter<SwaggerOperationFilter>();
        }

        public class SwaggerOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Responses.Add("204", new OpenApiResponse { Description = "NoContent" });
                operation.Responses.Add("400", new OpenApiResponse { Description = "BadRequest" });
                operation.Responses.Add("404", new OpenApiResponse { Description = "NotFound" });
                operation.Responses.Add("500", new OpenApiResponse { Description = "InternalServerError" });

                // check for auth attribute and add responses if any
                if (context.MethodInfo.DeclaringType != null)
                {
                    var anonymousAttributes = context.MethodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .Union(context.MethodInfo.GetCustomAttributes(true))
                        .OfType<AllowAnonymousAttribute>();

                    if (!anonymousAttributes.Any())
                    {
                        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                    }
                }
            }
        }
    }
}