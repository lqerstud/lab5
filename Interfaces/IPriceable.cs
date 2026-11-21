namespace ProductBuilder.Interfaces;

/// <summary>
/// Объект, у которого есть цена.
/// </summary>
public interface IPriceable
{
    decimal Price { get; }
}