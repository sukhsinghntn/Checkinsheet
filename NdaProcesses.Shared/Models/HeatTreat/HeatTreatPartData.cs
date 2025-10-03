using System.Text.Json;
using System.Text.Json.Serialization;

namespace NDAProcesses.Shared.Models.HeatTreat;

public sealed class HeatTreatPartData
{
    [JsonPropertyName("PN")]
    public string PartNumber { get; set; } = string.Empty;

    [JsonPropertyName("BJ NHA")]
    public string? BjNonHardenedArea { get; set; }

    [JsonPropertyName("BJ OPD")]
    public string? BjSplineOpdSpecification { get; set; }

    [JsonPropertyName("Column2")]
    public double? BjSplineOpdNominal { get; set; }

    [JsonPropertyName("Column22")]
    public double? BjSplineOpdTolerance { get; set; }

    [JsonPropertyName("BJ OPD MIN")]
    public double? BjSplineOpdMinimum { get; set; }

    [JsonPropertyName("BJ OPD MAX")]
    public double? BjSplineOpdMaximum { get; set; }

    [JsonPropertyName("DOJ NHA")]
    public string? DojNonHardenedArea { get; set; }

    [JsonPropertyName("DOJ OPD")]
    public string? DojSplineOpdSpecification { get; set; }

    [JsonPropertyName("Column3")]
    public double? DojSplineOpdNominal { get; set; }

    [JsonPropertyName("Column4")]
    public double? DojSplineOpdTolerance { get; set; }

    [JsonPropertyName("DOJ OPD MIN")]
    public double? DojSplineOpdMinimum { get; set; }

    [JsonPropertyName("DOJ OPD MAX")]
    public double? DojSplineOpdMaximum { get; set; }

    [JsonPropertyName("HRC")]
    public string? HardnessSpecification { get; set; }

    [JsonPropertyName("Case Depth")]
    public string? CaseDepthSpecification { get; set; }

    [JsonPropertyName("Clip Groove to Clip Groove")]
    public string? ClipGrooveSpecification { get; set; }

    [JsonPropertyName("Column5")]
    public double? ClipGrooveNominal { get; set; }

    [JsonPropertyName("Column6")]
    public double? ClipGrooveTolerance { get; set; }

    [JsonPropertyName("C to C MIN")]
    public double? ClipGrooveMinimum { get; set; }

    [JsonPropertyName("C to C MAX")]
    public double? ClipGrooveMaximum { get; set; }

    [JsonPropertyName("C to C Measure Location")]
    public string? ClipGrooveMeasureLocation { get; set; }

    [JsonPropertyName("X")]
    public double? XValue { get; set; }

    [JsonPropertyName("V")]
    public string? VValue { get; set; }

    [JsonPropertyName("ADCDE(half)FG")]
    public string? AdcdeHalfFg { get; set; }

    [JsonPropertyName("ABCDE(full)FG")]
    public string? AbcdeFullFg { get; set; }

    [JsonPropertyName("Hollow shaft")]
    public string? HollowShaft { get; set; }

    [JsonPropertyName("BJ Clip Ring Gauge")]
    public string? BjClipRingGauge { get; set; }

    [JsonPropertyName("BJ H Gauge")]
    public string? BjHGauge { get; set; }

    [JsonPropertyName("DOJ Clip Ring Gauge")]
    public string? DojClipRingGauge { get; set; }

    [JsonPropertyName("DOJ H Gauge")]
    public string? DojHGauge { get; set; }

    [JsonPropertyName("BJ Clip Width")]
    public string? BjClipWidth { get; set; }

    [JsonPropertyName("DOJ Clip Width")]
    public string? DojClipWidth { get; set; }

    [JsonPropertyName("Special Instructions")]
    public string? SpecialInstructions { get; set; }

    [JsonPropertyName("A Start")]
    public double? AStart { get; set; }

    [JsonPropertyName("A End")]
    public double? AEnd { get; set; }

    [JsonPropertyName("B Start")]
    public double? BStart { get; set; }

    [JsonPropertyName("B End")]
    public double? BEnd { get; set; }

    [JsonPropertyName("C Start")]
    public double? CStart { get; set; }

    [JsonPropertyName("C End")]
    public double? CEnd { get; set; }

    [JsonPropertyName("D Start")]
    public double? DStart { get; set; }

    [JsonPropertyName("D End")]
    public double? DEnd { get; set; }

    [JsonPropertyName("E Start")]
    public double? EStart { get; set; }

    [JsonPropertyName("E End")]
    public double? EEnd { get; set; }

    [JsonPropertyName("F Start")]
    public double? FStart { get; set; }

    [JsonPropertyName("F End")]
    public double? FEnd { get; set; }

    [JsonPropertyName("G Start")]
    public double? GStart { get; set; }

    [JsonPropertyName("G End")]
    public double? GEnd { get; set; }

    [JsonPropertyName("H Start")]
    public double? HStart { get; set; }

    [JsonPropertyName("H End")]
    public double? HEnd { get; set; }

    [JsonPropertyName("I Start")]
    public double? IStart { get; set; }

    [JsonPropertyName("I End")]
    public double? IEnd { get; set; }

    [JsonPropertyName("Customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("EddySonix")]
    public string? EddySonixRequirement { get; set; }

    [JsonPropertyName("# Cut Pcs")]
    public double? PiecesToCut { get; set; }

    [JsonPropertyName("CQI Cust")]
    public string? CqiCustomerCategory { get; set; }

    [JsonPropertyName("ES Approved Program")]
    public string? EsApprovedProgram { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}
