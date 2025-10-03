using System;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;
using System.Net.Http.Json;

namespace NDAProcesses.Client.Services;

public class HeatTreatServiceProxy : IHeatTreatService
{
    private readonly HttpClient _httpClient;

    public HeatTreatServiceProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<HeatTreatMasterRecord>> GetMasterRecordsAsync(CancellationToken cancellationToken = default)
    {
        var records = await _httpClient.GetFromJsonAsync<List<HeatTreatMasterRecord>>("api/HeatTreat", cancellationToken: cancellationToken);
        return records ?? [];
    }

    public Task<HeatTreatMasterRecord?> GetMasterRecordAsync(string partNumber, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<HeatTreatMasterRecord>($"api/HeatTreat/{Uri.EscapeDataString(partNumber)}", cancellationToken: cancellationToken);
    }
}
