using Microsoft.EntityFrameworkCore;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;
using NDAProcesses.Server.Data;

namespace NDAProcesses.Server.Services;

public class HeatTreatService : IHeatTreatService
{
    private readonly IDbContextFactory<HeatTreatDbContext> _dbContextFactory;
    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);
    private IReadOnlyDictionary<string, HeatTreatMasterRecord>? _cache;

    public HeatTreatService(IDbContextFactory<HeatTreatDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
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

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var records = await dbContext.HeatTreatMasterRecords
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            _cache = records
                .Where(record => !string.IsNullOrWhiteSpace(record.PartNumber))
                .Select(record =>
                {
                    record.PartNumber = record.PartNumber.Trim();
                    return record;
                })
                .GroupBy(record => record.PartNumber, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
            return _cache;
        }
        finally
        {
            _loadSemaphore.Release();
        }
    }
}
