namespace ProductBuilder.Models;

/// <summary>Сортировка по содержанию сахара на 100 г.</summary>
public class SweetSugarComparer : IComparer<Sweet>
{
    public int Compare(Sweet? x, Sweet? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return x.SugarPer100Grams.CompareTo(y.SugarPer100Grams);
    }
}

/// <summary>Сортировка по названию.</summary>
public class SweetNameComparer : IComparer<Sweet>
{
    public int Compare(Sweet? x, Sweet? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}