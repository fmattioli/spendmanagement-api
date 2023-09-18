using SpendManagement.API.Extensions;
using SpendManagement.Infra.CrossCutting.Conf;
using SpendManagement.Infra.CrossCutting.Extensions.HealthCheckers;
using SpendManagement.Infra.CrossCutting.Extensions.Kafka;
using SpendManagement.Infra.CrossCutting.Extensions.Logging;
using SpendManagement.Infra.CrossCutting.Extensions.Requests;
using SpendManagement.Infra.CrossCutting.Extensions.Services;
using SpendManagement.Infra.CrossCutting.Extensions.Tracing;
using SpendManagement.Infra.CrossCutting.Extensions.Validators;
using SpendManagement.Infra.CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddFilter("Microsoft", LogLevel.Critical);
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddSingleton<ISettings>(applicationSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddTracing()
    .AddKafka(applicationSettings.KafkaSettings)
    .AddAuthorization(applicationSettings.TokenAuth)
    .AddDependencyInjection()
    .AddLoggingDependency()
    .AddValidators()
    .AddHttpClients(applicationSettings.SpendManagementReadModel)
    .AddServices()
    .AddHealthChecks(applicationSettings)
    .AddControllers()
    .AddNewtonsoftJson()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressInferBindingSourcesForParameters = true;
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger();

var app = builder.Build();

//Add exception middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendManagement.API"));
app.UseHealthCheckers();
app.UseHttpsRedirection();
app.UseHeaderPropagation();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.ShowKafkaDashboard();
app.Run();
