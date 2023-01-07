using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Spents.Infra.CrossCutting.Conf;

namespace Spents.Infra.CrossCutting.Extensions.Mongo
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings mongoSettings)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var client = new MongoClient(mongoSettings.ConnectionString);
                var db = client?.GetDatabase(mongoSettings?.Database);
                return client;
            });

            services.AddTransient(sp =>
            {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
                var options = sp.GetService<MongoSettings>();
                var mongoClient = sp.GetService<IMongoClient>();
                var db = mongoClient?.GetDatabase(options?.Database);
                return db;
            });

            return services;
        }
    }
}
