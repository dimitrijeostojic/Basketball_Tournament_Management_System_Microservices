using Domain.Entities;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Seed;

public sealed class StadiumSeeder(
    IOptions<MongoDbOptions> options,
    ILogger<StadiumSeeder> logger)
{
    public async Task SeedAsync()
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        var collection = database.GetCollection<Stadium>(options.Value.CollectionName);

        var count = await collection.CountDocumentsAsync(_ => true);
        if (count > 0)
        {
            logger.LogInformation("Stadiums already seeded, skipping.");
            return;
        }

        var stadiums = new List<Stadium>
        {
            Stadium.Create("Lusail Iconic Stadium", "Lusail", 88966),
            Stadium.Create("Al Bayt Stadium", "Al Khor", 60000),
            Stadium.Create("Khalifa International Stadium", "Doha", 45416),
            Stadium.Create("Al Thumama Stadium", "Doha", 40000),
            Stadium.Create("Ahmad Bin Ali Stadium", "Al Rayyan", 44740),
            Stadium.Create("Al Janoub Stadium", "Al Wakrah", 40000),
            Stadium.Create("Education City Stadium", "Al Rayyan", 45350),
            Stadium.Create("Stadium 974", "Doha", 44089)
        };

        await collection.InsertManyAsync(stadiums);
        logger.LogInformation("Seeded {Count} stadiums.", stadiums.Count);
    }
}