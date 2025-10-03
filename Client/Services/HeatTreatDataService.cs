using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NDAProcesses.Shared.Models.HeatTreat;

namespace NDAProcesses.Client.Services;

public sealed class HeatTreatDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HeatTreatDataService> _logger;

    private IReadOnlyList<HeatTreatPartData>? _parts;
    private HeatTreatListReferenceData? _references;

    public HeatTreatDataService(HttpClient httpClient, ILogger<HeatTreatDataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<HeatTreatPartData>> GetPartsAsync(CancellationToken cancellationToken = default)
    {
        if (_parts is not null)
        {
            return _parts;
        }

        try
        {
            var parts = await _httpClient.GetFromJsonAsync<List<HeatTreatPartData>>("data/heatTreatMaster.json", cancellationToken);
            _parts = parts?.OrderBy(p => p.PartNumber).ToList() ?? new List<HeatTreatPartData>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load Heat Treat master data");
            _parts = Array.Empty<HeatTreatPartData>();
        }

        return _parts;
    }

    public async Task<HeatTreatListReferenceData> GetListReferencesAsync(CancellationToken cancellationToken = default)
    {
        if (_references is not null)
        {
            return _references;
        }

        try
        {
            _references = await _httpClient.GetFromJsonAsync<HeatTreatListReferenceData>("data/heatTreatListReferences.json", cancellationToken)
                ?? new HeatTreatListReferenceData();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load Heat Treat reference data");
            _references = new HeatTreatListReferenceData();
        }

        return _references;
    }

    public async Task<HeatTreatPartData?> FindPartAsync(string? partNumber, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(partNumber))
        {
            return null;
        }

        var parts = await GetPartsAsync(cancellationToken);
        return parts.FirstOrDefault(part => string.Equals(part.PartNumber, partNumber, StringComparison.OrdinalIgnoreCase));
    }
}
