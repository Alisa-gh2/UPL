class Program
{
    static void Main()
    {
        Console.WriteLine("Введите первое слово");
        string word1 = Console.ReadLine();
        Console.WriteLine("Введите второе слово");
        string word2 = Console.ReadLine();
        string content = File.ReadAllText("C:\\.ВРОДЕ ЛОКАЛЬНЫЙ ДИСК Х\\Учеба 1 курс\\Учебно-лабораторный практикум\\randomtext.txt");
        content = content.Replace(word1, word2);
        File.WriteAllText("C:\\.ВРОДЕ ЛОКАЛЬНЫЙ ДИСК Х\\Учеба 1 курс\\Учебно-лабораторный практикум\\randomtext.txt", content);
        Console.WriteLine("Замена выполнена");
    }
}