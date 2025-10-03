using Microsoft.AspNetCore.Mvc;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;

namespace NDAProcesses.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HeatTreatController : ControllerBase
{
    private readonly IHeatTreatService _heatTreatService;

    public HeatTreatController(IHeatTreatService heatTreatService)
    {
        _heatTreatService = heatTreatService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HeatTreatMasterRecord>>> GetRecords(CancellationToken cancellationToken)
    {
        var records = await _heatTreatService.GetMasterRecordsAsync(cancellationToken);
        return Ok(records);
    }

    [HttpGet("{partNumber}")]
    public async Task<ActionResult<HeatTreatMasterRecord>> GetRecord(string partNumber, CancellationToken cancellationToken)
    {
        var record = await _heatTreatService.GetMasterRecordAsync(partNumber, cancellationToken);
        if (record is null)
        {
            return NotFound();
        }

        return Ok(record);
    }
}
