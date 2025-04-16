using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// Объявляем интерфейс ISwimable
public interface ISwimable
{
    void Swim();
}

public class Program
{
    // Переносим JsonOptions внутрь класса
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { AddTypeDiscriminator }
        }
    };

    private static void AddTypeDiscriminator(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type != typeof(Animal)) return;

        typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
        {
            TypeDiscriminatorPropertyName = "$type",
            IgnoreUnrecognizedTypeDiscriminators = true,
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            DerivedTypes =
        {
            new JsonDerivedType(typeof(Fish), "Fish"),
            new JsonDerivedType(typeof(Penguin), "Penguin"),
            new JsonDerivedType(typeof(Dolphin), "Dolphin")
        }
        };
    }
    static void Main()
    {
        string filePath = "animals.json";

        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
        // Очищаем файл для тестирования
        if (File.Exists(filePath))
            File.Delete(filePath);

        // Создаем животных и добавляем их
        AddAnimal(filePath, new Fish
        {
            Title = "Карась",
            Age = 1,
            WaterType = "пресная"
        });

        AddAnimal(filePath, new Penguin
        {
            Title = "Императорский пингвин",
            Age = 7,
            FeatherColor = "черно-белый",
            SlideSpeed = 10
        });

        AddAnimal(filePath, new Dolphin
        {
            Title = "Атлантический дельфин",
            Age = 10,
            Habitat = "океан",
            Sound = "щелчки"
        });

        // Тестируем операции
        FindAnimal(filePath, "Карась");
        RemoveAnimal(filePath, "Императорский пингвин");
        FindAnimal(filePath, "Императорский пингвин");
        Console.WriteLine($"Файл сохранен по пути: {Path.GetFullPath(filePath)}");
    }

    static void AddAnimal(string filePath, Animal animal)
    {
        Zoo zoo = LoadZoo(filePath);
        zoo.Animals.Add(animal);

        string json = JsonSerializer.Serialize(zoo, JsonOptions);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"{animal.Title} добавлен в зоопарк.");
    }

    static void RemoveAnimal(string filePath, string title)
    {
        Zoo zoo = LoadZoo(filePath);
        int removed = zoo.Animals.RemoveAll(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (removed > 0)
        {
            string json = JsonSerializer.Serialize(zoo, JsonOptions);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"{title} удален из зоопарка.");
        }
        else
        {
            Console.WriteLine($"{title} не найден в зоопарке.");
        }
    }

    static void FindAnimal(string filePath, string title)
    {
        Zoo zoo = LoadZoo(filePath);
        Animal animal = zoo.Animals.Find(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (animal != null)
        {
            Console.WriteLine("Найдено животное:");
            animal.PrintProperties();

            if (animal is ISwimable swimmer)
            {
                swimmer.Swim();
            }
        }
        else
        {
            Console.WriteLine($"{title} не найден в зоопарке.");
        }
    }

    static Zoo LoadZoo(string filePath)
    {
        if (!File.Exists(filePath))
            return new Zoo();

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Zoo>(json, JsonOptions) ?? new Zoo();
    }
}

// Конвертер для полиморфной десериализации
public class AnimalJsonConverter : JsonConverter<Animal>
{
    public override Animal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty("$type", out var typeProp))
        {
            string typeName = typeProp.GetString();
            return typeName switch
            {
                "Fish" => JsonSerializer.Deserialize<Fish>(root.GetRawText(), options),
                "Penguin" => JsonSerializer.Deserialize<Penguin>(root.GetRawText(), options),
                "Dolphin" => JsonSerializer.Deserialize<Dolphin>(root.GetRawText(), options),
                _ => throw new JsonException($"Unknown type: {typeName}")
            };
        }

        throw new JsonException("Type discriminator ($type) not found");
    }

    public override void Write(Utf8JsonWriter writer, Animal value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Добавляем информацию о типе
        writer.WriteString("$type", value.GetType().Name);

        // Сериализуем свойства
        switch (value)
        {
            case Fish fish:
                writer.WriteString("Title", fish.Title);
                writer.WriteNumber("Age", fish.Age);
                writer.WriteString("WaterType", fish.WaterType);
                break;

            case Penguin penguin:
                writer.WriteString("Title", penguin.Title);
                writer.WriteNumber("Age", penguin.Age);
                writer.WriteString("FeatherColor", penguin.FeatherColor);
                writer.WriteNumber("SlideSpeed", penguin.SlideSpeed);
                break;

            case Dolphin dolphin:
                writer.WriteString("Title", dolphin.Title);
                writer.WriteNumber("Age", dolphin.Age);
                writer.WriteString("Habitat", dolphin.Habitat);
                writer.WriteString("Sound", dolphin.Sound);
                break;
        }

        writer.WriteEndObject();
    }
}

// Базовые классы
[JsonPolymorphic]
[JsonDerivedType(typeof(Fish), nameof(Fish))]
[JsonDerivedType(typeof(Penguin), nameof(Penguin))]
[JsonDerivedType(typeof(Dolphin), nameof(Dolphin))]
public abstract class Animal
{
    public string Title { get; set; }
    public int Age { get; set; }
    public abstract void PrintProperties();
}

public class Zoo
{
    public List<Animal> Animals { get; set; } = new List<Animal>();
}

// Конкретные классы животных
public class Fish : Animal, ISwimable
{
    public string WaterType { get; set; }

    public override void PrintProperties()
    {
        Console.WriteLine($"Рыба: {Title}");
        Console.WriteLine($"Возраст: {Age} лет");
        Console.WriteLine($"Тип воды: {WaterType}");
    }

    public void Swim()
    {
        Console.WriteLine($"{Title} плавает в {WaterType} воде");
    }
}

public class Penguin : Animal, ISwimable
{
    public string FeatherColor { get; set; }
    public int SlideSpeed { get; set; }

    public override void PrintProperties()
    {
        Console.WriteLine($"Пингвин: {Title}");
        Console.WriteLine($"Возраст: {Age} лет");
        Console.WriteLine($"Цвет перьев: {FeatherColor}");
        Console.WriteLine($"Скорость скольжения: {SlideSpeed} км/ч");
    }

    public void Swim()
    {
        Console.WriteLine($"{Title} отлично плавает и скользит");
    }
}

public class Dolphin : Animal, ISwimable
{
    public string Habitat { get; set; }
    public string Sound { get; set; }

    public override void PrintProperties()
    {
        Console.WriteLine($"Дельфин: {Title}");
        Console.WriteLine($"Возраст: {Age} лет");
        Console.WriteLine($"Среда обитания: {Habitat}");
        Console.WriteLine($"Издаваемый звук: {Sound}");
    }

    public void Swim()
    {
        Console.WriteLine($"{Title} быстро плавает в {Habitat}");
    }

    ~Dolphin()
    {
        Console.WriteLine($"Дельфин {Title} уплыл...");
    }
}