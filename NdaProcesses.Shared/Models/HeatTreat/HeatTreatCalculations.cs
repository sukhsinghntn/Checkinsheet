using System.Collections.Generic;

namespace NDAProcesses.Shared.Models.HeatTreat;

public static class HeatTreatCalculations
{
    private static readonly string[] TopSplinePartIndicators = ["BJ117", "BJ125"];

    private static readonly HashSet<string> GroupA = new(["H-1", "H-2", "H-3", "H-4", "H-5"], StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> GroupB = new(["H-13", "H-14", "H-15", "H-16"], StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> GroupC = new(["H-17", "H-18", "H-19", "H-6"], StringComparer.OrdinalIgnoreCase);

    public static bool UsesTopSplineLabel(string? partNumber)
    {
        if (string.IsNullOrWhiteSpace(partNumber))
        {
            return false;
        }

        return TopSplinePartIndicators.Any(indicator => partNumber.Contains(indicator, StringComparison.OrdinalIgnoreCase));
    }

    public static SpecificationRange BuildBjSplineRange(HeatTreatPartData part)
    {
        if (part.BjSplineOpdMinimum.HasValue || part.BjSplineOpdMaximum.HasValue)
        {
            return new SpecificationRange(part.BjSplineOpdMinimum, part.BjSplineOpdMaximum);
        }

        return HeatTreatSpecificationParser.FromPlusMinus(part.BjSplineOpdNominal, part.BjSplineOpdTolerance);
    }

    public static SpecificationRange BuildDojSplineRange(HeatTreatPartData part)
    {
        if (part.DojSplineOpdMinimum.HasValue || part.DojSplineOpdMaximum.HasValue)
        {
            return new SpecificationRange(part.DojSplineOpdMinimum, part.DojSplineOpdMaximum);
        }

        return HeatTreatSpecificationParser.FromPlusMinus(part.DojSplineOpdNominal, part.DojSplineOpdTolerance);
    }

    public static SpecificationRange BuildClipGrooveRange(HeatTreatPartData part)
    {
        if (part.ClipGrooveMinimum.HasValue || part.ClipGrooveMaximum.HasValue)
        {
            return new SpecificationRange(part.ClipGrooveMinimum, part.ClipGrooveMaximum);
        }

        return HeatTreatSpecificationParser.FromPlusMinus(part.ClipGrooveNominal, part.ClipGrooveTolerance);
    }

    public static SpecificationRange BuildCaseDepthRange(HeatTreatPartData part)
        => HeatTreatSpecificationParser.FromSpecString(part.CaseDepthSpecification);

    public static SpecificationRange BuildHardnessRange(HeatTreatPartData part)
        => HeatTreatSpecificationParser.FromSpecString(part.HardnessSpecification);

    public static SpecificationRange BuildGaugeWidthRange(string? gaugeSpec)
        => HeatTreatSpecificationParser.FromSpecString(gaugeSpec);

    public static int? CalculateNonCqiAxisOverride(
        string? cqiCategory,
        string? typeOfTest,
        string? line,
        string? shift,
        int? sampleSequence)
    {
        if (!string.Equals(cqiCategory, "Non-CQI Customer", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!string.Equals(typeOfTest, "8HR", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(line) || string.IsNullOrWhiteSpace(shift) || sampleSequence is null)
        {
            return null;
        }

        var primary =
            (GroupA.Contains(line) && string.Equals(shift, "2ND", StringComparison.OrdinalIgnoreCase)) ||
            (GroupB.Contains(line) && string.Equals(shift, "3RD", StringComparison.OrdinalIgnoreCase)) ||
            (GroupC.Contains(line) && string.Equals(shift, "1ST", StringComparison.OrdinalIgnoreCase));

        var secondary =
            (GroupA.Contains(line) && string.Equals(shift, "3RD", StringComparison.OrdinalIgnoreCase)) ||
            (GroupB.Contains(line) && string.Equals(shift, "1ST", StringComparison.OrdinalIgnoreCase)) ||
            (GroupC.Contains(line) && string.Equals(shift, "2ND", StringComparison.OrdinalIgnoreCase));

        if (!primary && !secondary)
        {
            return null;
        }

        return primary
            ? sampleSequence switch { 2 => 1, 1 => 2, _ => (int?)null }
            : sampleSequence switch { 2 => 3, 1 => 4, _ => (int?)null };
    }
}
