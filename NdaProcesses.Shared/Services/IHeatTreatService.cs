using NDAProcesses.Shared.Models;

namespace NDAProcesses.Shared.Services;

public interface IHeatTreatService
{
    Task<IReadOnlyList<HeatTreatMasterRecord>> GetMasterRecordsAsync(CancellationToken cancellationToken = default);

    Task<HeatTreatMasterRecord?> GetMasterRecordAsync(string partNumber, CancellationToken cancellationToken = default);
}
