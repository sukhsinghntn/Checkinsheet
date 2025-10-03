namespace NDAProcesses.Shared.Models.HeatTreat;

public readonly record struct SpecificationRange(double? Minimum, double? Maximum)
{
    public bool HasLimits => Minimum.HasValue || Maximum.HasValue;

    public bool? Evaluate(double? measurement)
    {
        if (!HasLimits || measurement is null)
        {
            return null;
        }

        if (Minimum.HasValue && measurement < Minimum.Value)
        {
            return false;
        }

        if (Maximum.HasValue && measurement > Maximum.Value)
        {
            return false;
        }

        return true;
    }
}
