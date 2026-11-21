using System.Text;
using ProductBuilder.Interfaces;
using ProductBuilder.Models;
using ProductBuilder.Services;

Console.OutputEncoding = Encoding.UTF8;

// 1. Ищем файл данных: сначала рядом с бинарником, потом относительно CWD.
var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "sweets.txt");
if (!File.Exists(dataPath))
    dataPath = Path.Combine("Data", "sweets.txt");

// 2. Загружаем каталог через фабрику.
var catalog = SweetFactory.LoadFromFile(dataPath);

Console.WriteLine("=== Каталог сладостей ===");
foreach (var s in catalog)
    Console.WriteLine("  " + s);

// 3. Собираем подарок через Builder: только сладости с сахаром < 80 г/100 г.
IGiftBuilder builder = new GiftBuilder();
var gift = builder
    .SetName("Новогодний подарок для Васи")
    .AddSweets(catalog.Where(s => s.SugarPer100Grams < 80))
    .Build();

Console.WriteLine();
Console.WriteLine($"=== {gift.Name} ===");
Console.WriteLine($"Конфет: {gift.Count}");
Console.WriteLine($"Общий вес: {gift.TotalWeightGrams:F1} г");
Console.WriteLine($"Всего сахара: {gift.TotalSugarGrams:F1} г");
Console.WriteLine($"Стоимость: {gift.TotalPrice:F2} ₽");

Console.WriteLine();
Console.WriteLine("--- Содержимое ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);

// 4. Сортировка по весу (IComparable<Sweet>).
gift.SortByWeight();
Console.WriteLine();
Console.WriteLine("--- Сортировка по весу (IComparable) ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);

// 5. Сортировка по сахару (IComparer<Sweet>).
gift.SortBySugar();
Console.WriteLine();
Console.WriteLine("--- Сортировка по сахару (IComparer) ---");
foreach (var s in gift)
    Console.WriteLine("  " + s);

// 6. Поиск конфет по диапазону сахара.
const double minSugar = 60, maxSugar = 75;
Console.WriteLine();
Console.WriteLine($"--- Конфеты с сахаром от {minSugar} до {maxSugar} г/100г ---");
var found = gift.FindBySugarRange(minSugar, maxSugar).ToList();
if (found.Count == 0)
    Console.WriteLine("  Не найдено");
else
    foreach (var s in found)
        Console.WriteLine("  " + s);