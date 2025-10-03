using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NDAProcesses.Shared.Models;

namespace NDAProcesses.Server.Data;

public class HeatTreatMasterDataInitializer
{
    private readonly HeatTreatDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<HeatTreatMasterDataInitializer> _logger;

    public HeatTreatMasterDataInitializer(
        HeatTreatDbContext context,
        IWebHostEnvironment environment,
        ILogger<HeatTreatMasterDataInitializer> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.EnsureCreatedAsync(cancellationToken);

        if (await _context.HeatTreatMasterRecords.AnyAsync(cancellationToken))
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
        var seedRecords = await JsonSerializer.DeserializeAsync<List<HeatTreatMasterRecord>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }, cancellationToken) ?? [];

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

        _context.HeatTreatMasterRecords.AddRange(normalizedRecords);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} heat treat master records from {Path}", normalizedRecords.Count, dataPath);
    }
}
