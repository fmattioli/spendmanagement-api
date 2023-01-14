using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
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
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoSettings>();
                return new MongoClient(mongoSettings.ConnectionString);
            });

            services.AddSingleton(sp =>
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                var mongoClient = sp.GetService<IMongoClient>();
                var db = mongoClient?.GetDatabase(mongoSettings.Database);
                return db;
            });

            return services;
        }
    }
}
