using System.Text.Json.Serialization;

namespace NDAProcesses.Shared.Models.HeatTreat;

public sealed class HeatTreatListReferenceData
{
    [JsonPropertyName("typeOfTestOptions")]
    public List<string> TypeOfTestOptions { get; set; } = new();

    [JsonPropertyName("shiftOptions")]
    public List<string> ShiftOptions { get; set; } = new();

    [JsonPropertyName("lineOptions")]
    public List<string> LineOptions { get; set; } = new();

    [JsonPropertyName("passFailOptions")]
    public List<string> PassFailOptions { get; set; } = new();

    [JsonPropertyName("hardnessSpecs")]
    public List<string> HardnessSpecifications { get; set; } = new();

    [JsonPropertyName("caseDepthRanges")]
    public List<string> CaseDepthRanges { get; set; } = new();

    [JsonPropertyName("judgementOptions")]
    public List<string> JudgementOptions { get; set; } = new();

    [JsonPropertyName("operatorIds")]
    public List<string> OperatorIdentifiers { get; set; } = new();

    [JsonPropertyName("operatorNames")]
    public List<string> OperatorNames { get; set; } = new();
}
