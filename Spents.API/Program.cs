using Spents.Infra.CrossCutting.Conf;
using Spents.Infra.CrossCutting.Extensions.Kafka;
using Spents.Infra.CrossCutting.Extensions.Mongo;
using Spents.API.Extensions;

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
    .AddDependecyInjection()
    .AddControllers();

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

app.Run();
