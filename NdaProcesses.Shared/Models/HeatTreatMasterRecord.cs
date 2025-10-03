using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NDAProcesses.Shared.Models;

public class HeatTreatMasterRecord
{
    [Key]
    [StringLength(128)]
    [JsonPropertyName("partNumber")]
    public string PartNumber { get; set; } = string.Empty;

    [JsonPropertyName("bjNonHardenedArea")]
    public string? BjNonHardenedArea { get; set; }

    [JsonPropertyName("bjSplineOpdDisplay")]
    public string? BjSplineOpdDisplay { get; set; }

    [JsonPropertyName("bjSplineOpdNominal")]
    public double? BjSplineOpdNominal { get; set; }

    [JsonPropertyName("bjSplineOpdTolerance")]
    public double? BjSplineOpdTolerance { get; set; }

    [JsonPropertyName("bjSplineOpdMin")]
    public double? BjSplineOpdMin { get; set; }

    [JsonPropertyName("bjSplineOpdMax")]
    public double? BjSplineOpdMax { get; set; }

    [JsonPropertyName("dojNonHardenedArea")]
    public string? DojNonHardenedArea { get; set; }

    [JsonPropertyName("dojSplineOpdDisplay")]
    public string? DojSplineOpdDisplay { get; set; }

    [JsonPropertyName("dojSplineOpdNominal")]
    public double? DojSplineOpdNominal { get; set; }

    [JsonPropertyName("dojSplineOpdTolerance")]
    public double? DojSplineOpdTolerance { get; set; }

    [JsonPropertyName("dojSplineOpdMin")]
    public double? DojSplineOpdMin { get; set; }

    [JsonPropertyName("dojSplineOpdMax")]
    public double? DojSplineOpdMax { get; set; }

    [JsonPropertyName("hardness")]
    public string? Hardness { get; set; }

    [JsonPropertyName("caseDepth")]
    public string? CaseDepth { get; set; }

    [JsonPropertyName("clipGrooveToClipGroove")]
    public string? ClipGrooveToClipGroove { get; set; }

    [JsonPropertyName("clipGrooveNominal")]
    public double? ClipGrooveNominal { get; set; }

    [JsonPropertyName("clipGrooveTolerance")]
    public double? ClipGrooveTolerance { get; set; }

    [JsonPropertyName("clipGrooveMin")]
    public double? ClipGrooveMin { get; set; }

    [JsonPropertyName("clipGrooveMax")]
    public double? ClipGrooveMax { get; set; }

    [JsonPropertyName("clipGrooveMeasureLocation")]
    public string? ClipGrooveMeasureLocation { get; set; }

    [JsonPropertyName("xValue")]
    public double? XValue { get; set; }

    [JsonPropertyName("vValue")]
    public string? VValue { get; set; }

    [JsonPropertyName("adcdeHalfFg")]
    public string? AdcdeHalfFg { get; set; }

    [JsonPropertyName("abcdeFullFg")]
    public string? AbcdeFullFg { get; set; }

    [JsonPropertyName("hollowShaft")]
    public string? HollowShaft { get; set; }

    [JsonPropertyName("abcdefgh1")]
    public string? Abcdefgh1 { get; set; }

    [JsonPropertyName("abcdefgh2")]
    public string? Abcdefgh2 { get; set; }

    [JsonPropertyName("abcdefgh3")]
    public string? Abcdefgh3 { get; set; }

    [JsonPropertyName("abcdefgh4")]
    public string? Abcdefgh4 { get; set; }

    [JsonPropertyName("abcdefgh5")]
    public string? Abcdefgh5 { get; set; }

    [JsonPropertyName("abcdefgh6")]
    public string? Abcdefgh6 { get; set; }

    [JsonPropertyName("abcdefgh7")]
    public string? Abcdefgh7 { get; set; }

    [JsonPropertyName("abcdefgh8")]
    public string? Abcdefgh8 { get; set; }

    [JsonPropertyName("abcdefgh9")]
    public string? Abcdefgh9 { get; set; }

    [JsonPropertyName("abcdefgh10")]
    public string? Abcdefgh10 { get; set; }

    [JsonPropertyName("abcdefghi1")]
    public string? Abcdefghi1 { get; set; }

    [JsonPropertyName("abcdefghi2")]
    public string? Abcdefghi2 { get; set; }

    [JsonPropertyName("abcdefghi3")]
    public string? Abcdefghi3 { get; set; }

    [JsonPropertyName("abcdefghi4")]
    public string? Abcdefghi4 { get; set; }

    [JsonPropertyName("column1")]
    public string? Column1 { get; set; }

    [JsonPropertyName("bjClipRingGauge")]
    public string? BjClipRingGauge { get; set; }

    [JsonPropertyName("bjHGauge")]
    public string? BjHGauge { get; set; }

    [JsonPropertyName("dojClipRingGauge")]
    public string? DojClipRingGauge { get; set; }

    [JsonPropertyName("dojHGauge")]
    public string? DojHGauge { get; set; }

    [JsonPropertyName("bjClipWidth")]
    public string? BjClipWidth { get; set; }

    [JsonPropertyName("dojClipWidth")]
    public string? DojClipWidth { get; set; }

    [JsonPropertyName("specialInstructions")]
    public string? SpecialInstructions { get; set; }

    [JsonPropertyName("aStart")]
    public string? AStart { get; set; }

    [JsonPropertyName("aEnd")]
    public string? AEnd { get; set; }

    [JsonPropertyName("bStart")]
    public string? BStart { get; set; }

    [JsonPropertyName("bEnd")]
    public string? BEnd { get; set; }

    [JsonPropertyName("cStart")]
    public string? CStart { get; set; }

    [JsonPropertyName("cEnd")]
    public string? CEnd { get; set; }

    [JsonPropertyName("dStart")]
    public string? DStart { get; set; }

    [JsonPropertyName("dEnd")]
    public string? DEnd { get; set; }

    [JsonPropertyName("eStart")]
    public string? EStart { get; set; }

    [JsonPropertyName("eEnd")]
    public string? EEnd { get; set; }

    [JsonPropertyName("fStart")]
    public string? FStart { get; set; }

    [JsonPropertyName("fEnd")]
    public string? FEnd { get; set; }

    [JsonPropertyName("gStart")]
    public string? GStart { get; set; }

    [JsonPropertyName("gEnd")]
    public string? GEnd { get; set; }

    [JsonPropertyName("hStart")]
    public string? HStart { get; set; }

    [JsonPropertyName("hEnd")]
    public string? HEnd { get; set; }

    [JsonPropertyName("iStart")]
    public string? IStart { get; set; }

    [JsonPropertyName("iEnd")]
    public string? IEnd { get; set; }

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("eddysonix")]
    public string? Eddysonix { get; set; }

    [JsonPropertyName("cutPieceCount")]
    public double? CutPieceCount { get; set; }

    [JsonPropertyName("cqiCustomer")]
    public string? CqiCustomer { get; set; }

    [JsonPropertyName("esApprovedProgram")]
    public string? EsApprovedProgram { get; set; }
}
