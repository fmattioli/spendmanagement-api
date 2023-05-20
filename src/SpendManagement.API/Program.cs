using Microsoft.OpenApi.Models;
using SpendManagement.API.Extensions;
using SpendManagement.Infra.CrossCutting.Conf;
using SpendManagement.Infra.CrossCutting.Extensions;
using SpendManagement.Infra.CrossCutting.Extensions.Kafka;
using SpendManagement.Infra.CrossCutting.Extensions.Validators;
using SpendManagement.Infra.CrossCutting.Filters;
using SpendManagement.Infra.CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hosting, config) =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    config
        .SetBasePath(currentDirectory)
        .AddJsonFile($"{currentDirectory}/appsettings.json");
});

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddSingleton<ISettings>(applicationSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddKafka(applicationSettings.KafkaSettings)
    .AddDependencyInjection()
    .AddLoggingDependency()
    .AddValidators()

    .AddControllers((options =>
    {
        options.Filters.Add(typeof(FilterRequestAttribute));
    }))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendManagement API", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SpendManagement.API.xml"));
});

var app = builder.Build();

//Add exception middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.ShowKafkaDashboard();
app.Run();
