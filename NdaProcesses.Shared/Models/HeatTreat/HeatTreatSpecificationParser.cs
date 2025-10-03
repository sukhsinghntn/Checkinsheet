using System.Globalization;
using System.Text.RegularExpressions;

namespace NDAProcesses.Shared.Models.HeatTreat;

public static partial class HeatTreatSpecificationParser
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private static double? ParseDouble(string input)
    {
        if (double.TryParse(input, NumberStyles.Float, Culture, out var value))
        {
            return value;
        }

        return null;
    }

    public static SpecificationRange FromPlusMinus(double? nominal, double? tolerance)
    {
        if (nominal is null || tolerance is null)
        {
            return new SpecificationRange(null, null);
        }

        return new SpecificationRange(nominal - tolerance, nominal + tolerance);
    }

    public static SpecificationRange FromSpecString(string? specification)
    {
        if (string.IsNullOrWhiteSpace(specification))
        {
            return new SpecificationRange(null, null);
        }

        var value = specification.Trim();

        var plusMinusMatch = PlusMinusRegex().Match(value);
        if (plusMinusMatch.Success)
        {
            var nominal = ParseDouble(plusMinusMatch.Groups[1].Value);
            var tolerance = ParseDouble(plusMinusMatch.Groups[2].Value);
            return FromPlusMinus(nominal, tolerance);
        }

        var minMaxMatch = MinMaxRegex().Match(value);
        if (minMaxMatch.Success)
        {
            var min = ParseDouble(minMaxMatch.Groups[1].Value);
            var max = ParseDouble(minMaxMatch.Groups[2].Value);
            return new SpecificationRange(min, max);
        }

        var numericMatches = NumericRegex().Matches(value);
        if (numericMatches.Count == 2)
        {
            var min = ParseDouble(numericMatches[0].Value);
            var max = ParseDouble(numericMatches[1].Value);
            return new SpecificationRange(min, max);
        }

        if (numericMatches.Count == 1)
        {
            var target = ParseDouble(numericMatches[0].Value);
            return target.HasValue
                ? new SpecificationRange(target.Value, target.Value)
                : new SpecificationRange(null, null);
        }

        return new SpecificationRange(null, null);
    }

    [GeneratedRegex(@"([+-]?\d+(?:\.\d+)?)\s*[±\u00b1]\s*(\d+(?:\.\d+)?)", RegexOptions.IgnoreCase)]
    private static partial Regex PlusMinusRegex();

    [GeneratedRegex(@"([+-]?\d+(?:\.\d+)?)\s*-\s*([+-]?\d+(?:\.\d+)?)", RegexOptions.IgnoreCase)]
    private static partial Regex MinMaxRegex();

    [GeneratedRegex(@"[+-]?\d+(?:\.\d+)?", RegexOptions.IgnoreCase)]
    private static partial Regex NumericRegex();
}
