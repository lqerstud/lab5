namespace ProductBuilder.Models;

/// <summary>
/// Абстрактная конфета — у всех конфет общий атрибут «вкус».
/// Наследование оправдано: без него Flavor дублировался бы в каждом подклассе.
/// </summary>
public abstract class Candy : Sweet
{
    public string Flavor { get; }

    protected Candy(string name, double weight, double sugar, decimal pricePerKg, string flavor)
        : base(name, weight, sugar, pricePerKg)
    {
        Flavor = flavor;
    }
}