using ProductBuilder.Models;
using ProductBuilder.Services;
using Xunit;

namespace ProductBuilder.Tests;

public class ProductTests
{
    // ==========================================
    // Тесты для базового класса Sweet
    // ==========================================
    
    [Fact]
    public void Sweet_CalculatesPriceAndSugarCorrectly()
    {
        // Arrange (Подготовка)
        var candy = new ChocolateCandy("Тест", weight: 100, sugar: 50, pricePerKg: 1000, "ваниль", "молочный");

        // Act & Assert (Действие и Проверка)
        Assert.Equal(50.0, candy.SugarGrams); // 50 г сахара на 100 г веса
        Assert.Equal(100.0m, candy.Price);    // 1000 руб за кг * 100 г / 1000 = 100 руб
    }

    [Theory]
    [InlineData("", 10, 50, 100)]
    [InlineData("Конфета", -5, 50, 100)]
    [InlineData("Конфета", 10, 150, 100)]
    [InlineData("Конфета", 10, 50, -10)]
    public void Sweet_ThrowsException_OnInvalidInput(string name, double weight, double sugar, decimal price)
    {
        // Проверяем, что неверные данные вызывают исключения
        Assert.ThrowsAny<ArgumentException>(() => 
            new Lollipop(name, weight, sugar, price, "вкус"));
    }

    // ==========================================
    // Тесты для класса Gift
    // ==========================================

    [Fact]
    public void Gift_CalculatesTotalsCorrectly()
    {
        var gift = new Gift("Тестовый подарок");
        gift.Add(new Lollipop("Леденец 1", 50, 20, 500, "яблоко"));
        gift.Add(new Marmalade("Мармелад", 50, 80, 500, "апельсин"));

        Assert.Equal(2, gift.Count);
        Assert.Equal(100, gift.TotalWeightGrams);
        Assert.Equal(50, gift.TotalSugarGrams); // (50*0.2) + (50*0.8) = 10 + 40 = 50
        Assert.Equal(50m, gift.TotalPrice);     // 25 + 25 = 50
    }

    [Fact]
    public void Gift_FindBySugarRange_ReturnsCorrectSweets()
    {
        var gift = new Gift("Тест");
        gift.Add(new Lollipop("Мало сахара", 10, 10, 100, "вкус"));
        gift.Add(new Lollipop("Средне сахара", 10, 50, 100, "вкус"));
        gift.Add(new Lollipop("Много сахара", 10, 90, 100, "вкус"));

        var found = gift.FindBySugarRange(20, 60).ToList();

        Assert.Single(found);
        Assert.Equal("Средне сахара", found[0].Name);
    }

    // ==========================================
    // Тесты для GiftBuilder (Паттерн Builder)
    // ==========================================

    [Fact]
    public void GiftBuilder_BuildsGiftCorrectly()
    {
        var builder = new GiftBuilder();
        var sweet = new CaramelCandy("Барбарис", 10, 70, 300, "барбарис", true);

        var gift = builder
            .SetName("Мой подарок")
            .AddSweet(sweet)
            .Build();

        Assert.Equal("Мой подарок", gift.Name);
        Assert.Single(gift.Sweets);
        Assert.Equal("Барбарис", gift.Sweets[0].Name);
    }

    // ==========================================
    // Тесты для SweetFactory (Паттерн Factory)
    // ==========================================

    [Fact]
    public void SweetFactory_ParsesStringCorrectly()
    {
        // Arrange: строка имитирует одну строку из файла Data/sweets.txt
        string line = "Chocolate|Алёнка|20|50|950|сливки|тёмный";

        // Act
        var sweet = SweetFactory.CreateFromLine(line);

        // Assert
        Assert.IsType<ChocolateCandy>(sweet);
        Assert.Equal("Алёнка", sweet.Name);
        Assert.Equal(20, sweet.WeightGrams);
        Assert.Equal(50, sweet.SugarPer100Grams);
        Assert.Equal(950m, sweet.PricePerKilogram);
        
        var choco = (ChocolateCandy)sweet;
        Assert.Equal("сливки", choco.Flavor);
        Assert.Equal("тёмный", choco.ChocolateType);
    }
}