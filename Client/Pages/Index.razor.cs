using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NDAProcesses.Client.Services;
using NDAProcesses.Shared.Models.HeatTreat;
using Radzen;

namespace NDAProcesses.Client.Pages;

public partial class Index
{
    private readonly IReadOnlyList<string> _axisPoints = new[] { "1/1L", "2/1R", "3/2L", "4/2R" };
    private readonly IReadOnlyList<int> _sampleNumbers = Enumerable.Range(1, 6).ToList();

    private bool _isLoading = true;
    private IReadOnlyList<HeatTreatPartData> _parts = Array.Empty<HeatTreatPartData>();
    private HeatTreatListReferenceData _references = new();
    private HeatTreatPartData? _selectedPart;
    private string? _selectedPartNumber;
    private HeatTreatFormState _formState = new();

    [Inject]
    public HeatTreatDataService DataService { get; set; } = default!;

    [Inject]
    public NotificationService NotificationService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        foreach (var number in _sampleNumbers)
        {
            _formState.Samples.Add(new HeatTreatSampleInput
            {
                Number = number,
                BjMeasurements = _axisPoints.Select(point => new HeatTreatMeasurementInput { Point = point }).ToList(),
                DojMeasurements = _axisPoints.Select(point => new HeatTreatMeasurementInput { Point = point }).ToList()
            });
        }

        _references = await DataService.GetListReferencesAsync();
        _parts = await DataService.GetPartsAsync();

        _isLoading = false;
    }

    private async Task OnPartNumberChanged(string? value)
    {
        _selectedPartNumber = value;
        _selectedPart = await DataService.FindPartAsync(value);
        if (_selectedPart is null)
        {
            return;
        }

        _formState.PartNeedsEddySonixScan = !string.Equals(_selectedPart.EddySonixRequirement, "N/A", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(_selectedPart.EddySonixRequirement);

        StateHasChanged();
    }

    private void HandleSubmit()
    {
        if (_selectedPart is null)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Select a part", "Please choose a part number before recording measurements.");
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Form saved", "Measurements captured in the browser session.");
    }

    private SpecificationRange BjSplineRange => _selectedPart is null ? default : HeatTreatCalculations.BuildBjSplineRange(_selectedPart);
    private SpecificationRange DojSplineRange => _selectedPart is null ? default : HeatTreatCalculations.BuildDojSplineRange(_selectedPart);
    private SpecificationRange ClipGrooveRange => _selectedPart is null ? default : HeatTreatCalculations.BuildClipGrooveRange(_selectedPart);
    private SpecificationRange CaseDepthRange => _selectedPart is null ? default : HeatTreatCalculations.BuildCaseDepthRange(_selectedPart);
    private SpecificationRange HardnessRange => _selectedPart is null ? default : HeatTreatCalculations.BuildHardnessRange(_selectedPart);
    private SpecificationRange BjClipWidthRange => _selectedPart is null ? default : HeatTreatCalculations.BuildGaugeWidthRange(_selectedPart.BjClipWidth);
    private SpecificationRange DojClipWidthRange => _selectedPart is null ? default : HeatTreatCalculations.BuildGaugeWidthRange(_selectedPart.DojClipWidth);

    private bool? EvaluateBjMeasurement(HeatTreatMeasurementInput measurement) => BjSplineRange.Evaluate(measurement.Value);
    private bool? EvaluateDojMeasurement(HeatTreatMeasurementInput measurement) => DojSplineRange.Evaluate(measurement.Value);
    private bool? EvaluateClipGroove() => ClipGrooveRange.Evaluate(_formState.ClipGrooveMeasurement);
    private bool? EvaluateCaseDepth() => CaseDepthRange.Evaluate(_formState.CaseDepthMeasurement);
    private bool? EvaluateHardness(double? measurement) => HardnessRange.Evaluate(measurement);
    private bool? EvaluateBjClipWidth() => BjClipWidthRange.Evaluate(_formState.BjClipWidthMeasured);
    private bool? EvaluateDojClipWidth() => DojClipWidthRange.Evaluate(_formState.DojClipWidthMeasured);

    private int? CalculateNonCqiOverride()
        => HeatTreatCalculations.CalculateNonCqiAxisOverride(
            _selectedPart?.CqiCustomerCategory,
            _formState.TypeOfTest,
            _formState.Line,
            _formState.Shift,
            _formState.SampleSequence);

    private static string FormatSpecificationRange(SpecificationRange range)
        => range.HasLimits ? $"{range.Minimum:0.###} - {range.Maximum:0.###}" : "-";

    private static string FormatResult(bool? result)
        => result is null ? "" : result.Value ? "PASS" : "FAIL";
}

public sealed class HeatTreatFormState
{
    public DateTime? InspectionDate { get; set; } = DateTime.Today;
    public TimeSpan? InspectionTime { get; set; } = DateTime.Now.TimeOfDay;
    public string? Shift { get; set; }
    public string? Line { get; set; }
    public string? TypeOfTest { get; set; }
    public string? Inspector { get; set; }
    public string? PassStatus { get; set; }
    public string? AdjustmentsRequired { get; set; }
    public string? StampNumber { get; set; }
    public int? SampleSequence { get; set; }
    public double? ClipGrooveMeasurement { get; set; }
    public double? CaseDepthMeasurement { get; set; }
    public double? BjClipWidthMeasured { get; set; }
    public double? DojClipWidthMeasured { get; set; }
    public double? BjClipRingMeasured { get; set; }
    public double? DojClipRingMeasured { get; set; }
    public double? BjHGaugeMeasured { get; set; }
    public double? DojHGaugeMeasured { get; set; }
    public bool PartNeedsEddySonixScan { get; set; }
    public string? EddySonixJudgement { get; set; }
    public double? EddySonixX { get; set; }
    public double? EddySonixV { get; set; }
    public string? Notes { get; set; }
    public string? VerifiedBy { get; set; }
    public List<HeatTreatSampleInput> Samples { get; } = new();
}

public sealed class HeatTreatSampleInput
{
    public int Number { get; set; }
    public List<HeatTreatMeasurementInput> BjMeasurements { get; set; } = new();
    public List<HeatTreatMeasurementInput> DojMeasurements { get; set; } = new();
    public double? HardnessMeasurement { get; set; }
}

public sealed class HeatTreatMeasurementInput
{
    public string Point { get; set; } = string.Empty;
    public double? Value { get; set; }
}
