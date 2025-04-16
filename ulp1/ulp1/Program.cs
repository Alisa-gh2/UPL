using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Животные
        List<Animal> animals = new List<Animal>();

        // Создаем животных
        Fish fish = new Fish("Карась", 1, "пресная");
        Penguin penguin = new Penguin("Императорский пингвин", 7, "черно-белый", 10);
        Dolphin dolphin = new Dolphin("Атлантический пятнистый дельфин", 10, "океан", "щелчок");

        // Добавляем в список
        animals.Add(fish);
        animals.Add(penguin);
        animals.Add(dolphin);
        animals.Add(new Bird("Синица", 1, "синий"));
        animals.Add(new Mammal("Лев", 4, "саванна"));

        // Выводим информацию
        foreach (Animal a in animals)
        {
            a.Move();
            a.PrintProperties();
            Console.WriteLine();
        }

        // Кто умеет плавать
        List<ISwimable> swimmers = new List<ISwimable>();
        swimmers.Add(fish);
        swimmers.Add(penguin);
        swimmers.Add(dolphin);

        foreach (ISwimable s in swimmers)
        {
            s.Swim();
        }
    }
}

interface ISwimable
{
    void Swim();
}

interface IAnimal
{
    void Move();
    void PrintProperties();
}

abstract class Animal : IAnimal
{
    protected string name;
    protected int age;

    public Animal(string name, int age)
    {
        this.name = name;
        this.age = age;
    }

    public abstract void Move();

    public virtual void PrintProperties()
    {
        Console.WriteLine("Название: " + name);
        Console.WriteLine("Возраст: " + age);
    }
}

class Fish : Animal, ISwimable
{
    protected string waterType;

    public Fish(string name, int age, string waterType) : base(name, age)
    {
        this.waterType = waterType;
    }

    public override void Move()
    {
        Console.WriteLine("Рыба плавает.");
    }

    public void Swim()
    {
        Console.WriteLine(name + " плавает в воде");
    }

    public override void PrintProperties()
    {
        base.PrintProperties();
        Console.WriteLine("Живет в " + waterType + " воде");
    }
}

class Bird : Animal
{
    protected string featherColor;

    public Bird(string name, int age, string featherColor) : base(name, age)
    {
        this.featherColor = featherColor;
    }

    public override void Move()
    {
        Console.WriteLine("Птица летает.");
    }

    public override void PrintProperties()
    {
        base.PrintProperties();
        Console.WriteLine("Цвет: " + featherColor);
    }
}

class Penguin : Bird, ISwimable
{
    protected int slideSpeed;

    public Penguin(string name, int age, string featherColor, int slideSpeed)
        : base(name, age, featherColor)
    {
        this.slideSpeed = slideSpeed;
    }

    public override void Move()
    {
        Console.WriteLine("Пингвин ходит и скользит.");
    }

    public void Swim()
    {
        Console.WriteLine(name + " тоже умеет плавать");
    }

    public override void PrintProperties()
    {
        base.PrintProperties();
        Console.WriteLine("Скорость: " + slideSpeed);
    }
}

class Mammal : Animal
{
    protected string habitat;

    public Mammal(string name, int age, string habitat) : base(name, age)
    {
        this.habitat = habitat;
    }

    public override void Move()
    {
        Console.WriteLine("Млекопитающее двигается");
    }

    public override void PrintProperties()
    {
        base.PrintProperties();
        Console.WriteLine("Живет в " + habitat);
    }
}

class Dolphin : Mammal, ISwimable
{
    protected string sound;

    public Dolphin(string name, int age, string habitat, string sound)
        : base(name, age, habitat)
    {
        this.sound = sound;
    }

    public override void Move()
    {
        Console.WriteLine("Дельфин плавает быстро");
    }

    public void Swim()
    {
        Console.WriteLine(name + " отлично плавает");
    }

    public override void PrintProperties()
    {
        base.PrintProperties();
        Console.WriteLine("Издает звук: " + sound);
    }

    ~Dolphin()
    {
        Console.WriteLine("Дельфин уплыл...");
    }
}