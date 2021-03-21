# Content of the file Startup.cs

```csharp
public class Startup
{
    private const string CorsPolicyName = "CorsPolicy";

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        new Logging(Configuration, services, Environment).Setup();

        services
            .AddMvc()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
            })
            .AddCustomDataAnnotationsLocalization()
            .AddMvcLocalization();

        services
            .Configure<ApiBehaviorOptions>(x =>
            {
                x.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult();
            });

        services.AddI18N();
        services.AddRazorPages();

        DatabaseConfig.Setup(services, Configuration, Environment.IsDevelopment());
        RedisConfig.Setup(services, Configuration);

        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(SwaggerConfig.Apply);

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

        services.AddCoravelScheduler();

        services
            .AddTransient<DatabaseHealthCheck>()
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database")
            .AddRedis(Configuration.GetConnectionString("Redis"))
            .AddAzureBlobHealthCheck(Configuration)
            .AddAzureSmtpHealthCheck(Configuration, Environment);

        ServiceRegistration.Setup(services, Configuration, Environment);

        new AuthenticationConfig(services, Configuration)
            .SelfBearer()
            .TcoSso();

        new MessageBrokerConfig(services, Configuration)
            .Setup();

        services.AddHostedService<AppInitializeService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseErrorHandler(Environment).UseLoggingMiddleware();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors(CorsPolicyName);

        app.UseAuthentication();

        // TODO Maxim: MRF does not have this line. We should check the work with the line and without it
        app.UseAuthorization();

        HealthCheckCustomResponse.Setup(app);

        app.SetupSchedulerTasks();

        app.UseRequestLocalization();
        app.UseMiddleware<RequestLocalizationCookiesMiddleware>();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwagger();
        app.UseSwaggerUI(SwaggerConfig.ApplyUI);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Should be placed at the end of this method.
        app.UseMiddleware<DefaultNotFoundPageMiddleware>();
    }
}
```