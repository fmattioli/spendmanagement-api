using Microsoft.OpenApi.Models;
using SpendManagement.API.Extensions;
using SpendManagement.Infra.CrossCutting.Conf;
using SpendManagement.Infra.CrossCutting.Extensions;
using SpendManagement.Infra.CrossCutting.Extensions.Kafka;
using SpendManagement.Infra.CrossCutting.Extensions.Requests;
using SpendManagement.Infra.CrossCutting.Extensions.Validators;
using SpendManagement.Infra.CrossCutting.Filters;
using SpendManagement.Infra.CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddSingleton<ISettings>(applicationSettings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddKafka(applicationSettings.KafkaSettings)
    .AddDependencyInjection()
    .AddLoggingDependency()
    .AddValidators()
    .AddHttpClients(applicationSettings.SpendManagementReadModel)
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(FilterRequestAttribute));
    }).AddNewtonsoftJson()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressInferBindingSourcesForParameters = true;
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendManagement API", Version = "v1", Description = "The completed platform to handle receipts related to the SpendMagement project." });
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SpendManagement.API.xml"));
    })
    .AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

//Add exception middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api One V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.ShowKafkaDashboard();
app.Run();
