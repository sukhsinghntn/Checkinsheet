using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Serialization;

namespace NDAProcesses.Server.Data;

public class HeatTreatMasterDataInitializer
{
    private readonly IDbContextFactory<HeatTreatDbContext> _dbContextFactory;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<HeatTreatMasterDataInitializer> _logger;

    public HeatTreatMasterDataInitializer(
        IDbContextFactory<HeatTreatDbContext> dbContextFactory,
        IWebHostEnvironment environment,
        ILogger<HeatTreatMasterDataInitializer> logger)
    {
        _dbContextFactory = dbContextFactory;
        _environment = environment;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.HeatTreatMasterRecords.AnyAsync(cancellationToken))
        {
            return;
        }

        var dataPath = Path.Combine(_environment.ContentRootPath, "Data", "heat-treat-master-data.json");
        if (!File.Exists(dataPath))
        {
            _logger.LogWarning("Heat treat master data seed file not found at {Path}", dataPath);
            return;
        }

        await using var stream = File.OpenRead(dataPath);
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        serializerOptions.Converters.Add(new FlexibleStringJsonConverter());
        serializerOptions.Converters.Add(new FlexibleNullableDoubleConverter());

        var seedRecords = await JsonSerializer.DeserializeAsync<List<HeatTreatMasterRecord>>(stream, serializerOptions, cancellationToken) ?? [];

        if (seedRecords.Count == 0)
        {
            _logger.LogWarning("Heat treat master data seed file at {Path} did not contain any records", dataPath);
            return;
        }

        var normalizedRecords = seedRecords
            .Where(record => !string.IsNullOrWhiteSpace(record.PartNumber))
            .Select(record =>
            {
                record.PartNumber = record.PartNumber.Trim();
                return record;
            })
            .GroupBy(record => record.PartNumber, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToList();

        if (normalizedRecords.Count == 0)
        {
            _logger.LogWarning("Heat treat master data seed produced no records after normalization");
            return;
        }

        context.HeatTreatMasterRecords.AddRange(normalizedRecords);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} heat treat master records from {Path}", normalizedRecords.Count, dataPath);
    }
}
