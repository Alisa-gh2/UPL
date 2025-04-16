using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

// Объявляем интерфейс ISwimable
public interface ISwimable
{
    void Swim();
}

public class Program
{
    static void Main()
    {
        string filePath = "animals.xml";

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

        var serializer = new XmlSerializer(typeof(Zoo));
        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, zoo);
        }

        Console.WriteLine($"{animal.Title} добавлен в зоопарк.");
    }

    static void RemoveAnimal(string filePath, string title)
    {
        Zoo zoo = LoadZoo(filePath);
        int removed = zoo.Animals.RemoveAll(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (removed > 0)
        {
            var serializer = new XmlSerializer(typeof(Zoo));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, zoo);
            }
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

        var serializer = new XmlSerializer(typeof(Zoo));
        using (var reader = new StreamReader(filePath))
        {
            return (Zoo)serializer.Deserialize(reader) ?? new Zoo();
        }
    }
}

// Базовые классы
[XmlInclude(typeof(Fish))]
[XmlInclude(typeof(Penguin))]
[XmlInclude(typeof(Dolphin))]
public abstract class Animal
{
    public string Title { get; set; }
    public int Age { get; set; }
    public abstract void PrintProperties();
}

[XmlRoot("Zoo")]
public class Zoo
{
    [XmlElement("Animal")]
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