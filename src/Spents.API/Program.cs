using Spents.API.Extensions;
using Spents.Infra.CrossCutting.Conf;
using Spents.Infra.CrossCutting.Extensions;
using Spents.Infra.CrossCutting.Extensions.Kafka;
using Spents.Infra.CrossCutting.Extensions.Mongo;
using Spents.Infra.CrossCutting.Extensions.Repositories;
using Spents.Infra.CrossCutting.Extensions.Validators;
using Spents.Infra.CrossCutting.Middlewares.Validators;

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


// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddKafka(applicationSettings.KafkaSettings)
    .AddMongo(applicationSettings.MongoSettings)
    .AddRepositories()
    .AddDependecyInjection()
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
