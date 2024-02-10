using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt;

namespace SpendManagement.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(AddReceiptCommand).Assembly,
                        typeof(AddRecurringReceiptCommand).Assembly,
                        typeof(UpdateReceiptCommand).Assembly));

            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, string? auth)
        {
            if (!string.IsNullOrEmpty(auth))
            {
                services.AddAuthentication(x =>
                 {
                     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 }).AddJwtBearer(x =>
                 {
                     x.RequireHttpsMetadata = false;
                     x.SaveToken = true;
                     x.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(auth)),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });
            }

            return services;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendManagement API", Version = "v1", Description = "The completed platform to handle receipts related to the SpendMagement project." });
                 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     In = ParameterLocation.Header,
                     Description = "Please insert token",
                     Name = "Authorization",
                     Type = SecuritySchemeType.Http,
                     Scheme = "Bearer"
                 });
                 c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                         Array.Empty<string>()
                     }
                 });
                 c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SpendManagement.API.xml"));
             });
        }
    }
}
