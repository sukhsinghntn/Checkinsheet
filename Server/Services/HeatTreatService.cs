using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;

namespace NDAProcesses.Server.Services;

public class HeatTreatService : IHeatTreatService
{
    private readonly IWebHostEnvironment _environment;
    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);
    private IReadOnlyDictionary<string, HeatTreatMasterRecord>? _cache;

    public HeatTreatService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<IReadOnlyList<HeatTreatMasterRecord>> GetMasterRecordsAsync(CancellationToken cancellationToken = default)
    {
        var cache = await GetOrLoadAsync(cancellationToken);
        return cache.Values
            .OrderBy(record => record.PartNumber, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<HeatTreatMasterRecord?> GetMasterRecordAsync(string partNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(partNumber))
        {
            return null;
        }

        var cache = await GetOrLoadAsync(cancellationToken);
        cache.TryGetValue(partNumber, out var record);
        return record;
    }

    private async Task<IReadOnlyDictionary<string, HeatTreatMasterRecord>> GetOrLoadAsync(CancellationToken cancellationToken)
    {
        if (_cache is not null)
        {
            return _cache;
        }

        await _loadSemaphore.WaitAsync(cancellationToken);
        try
        {
            if (_cache is not null)
            {
                return _cache;
            }

            var dataPath = Path.Combine(_environment.ContentRootPath, "Data", "heat-treat-master-data.json");
            await using var stream = File.OpenRead(dataPath);
            var records = await JsonSerializer.DeserializeAsync<List<HeatTreatMasterRecord>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken) ?? [];

            _cache = records.ToDictionary(record => record.PartNumber, StringComparer.OrdinalIgnoreCase);
            return _cache;
        }
        finally
        {
            _loadSemaphore.Release();
        }
    }
}
