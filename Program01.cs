using System;
using System.IO;

class Program
{
    static void Main()
    {
        Random rand = new Random();
        int[] arrue = new int[20];

        string inputFile = "C:\\.ВРОДЕ ЛОКАЛЬНЫЙ ДИСК Х\\Учеба 1 курс\\Учебно-лабораторный практикум\\test.txt";
        string outputFile = "C:\\.ВРОДЕ ЛОКАЛЬНЫЙ ДИСК Х\\Учеба 1 курс\\Учебно-лабораторный практикум\\test_results.txt";
        // 1. Запись 20 случайных чисел в файл
        using (StreamWriter st = new StreamWriter(inputFile, false))
        {
            for (int i = 0; i < 20; i++)
            {
                arrue[i] = rand.Next(-100, 100);
                st.WriteLine(arrue[i]);
            }
        }

        // 2. Поиск максимального числа
        int maxnumb = int.MinValue; // Начинаем с минимального возможного int
        using (StreamReader file = new StreamReader(inputFile))
        {
            string line;
            while ((line = file.ReadLine()) != null) // Читаем до конца файла
            {
                int current = Convert.ToInt32(line);
                if (current > maxnumb)
                {
                    maxnumb = current;
                }
            }
        }

        // 3. Вычисление половины максимума
        int determine = maxnumb / 2;
        Console.WriteLine("Половина максимального числа = " + determine);

        // 4. Чтение файла и вывод результатов деления на maxnumb
        using (StreamReader file2 = new StreamReader(inputFile))
        {
            string line;
            while ((line = file2.ReadLine()) != null)
            {
                int numb2 = Convert.ToInt32(line);
                Console.WriteLine("Исходное число: " + numb2);

                if (maxnumb != 0) // Проверка деления на ноль
                {
                    float numb3 = (float)numb2 / determine; // Дробное деление
                    Console.WriteLine("Результат деления на максимум: " + numb3);
                }
                else
                {
                    Console.WriteLine("Ошибка: максимальное число = 0, деление невозможно.");
                }
            }
        }

        // 5. Перезапись файла (если нужно)
        using (StreamReader file2 = new StreamReader(inputFile))
        using (StreamWriter results = new StreamWriter(outputFile, false))
        {
            string line;
            results.WriteLine("Исходное числоv| Результат деления ");
            while ((line = file2.ReadLine()) != null)
            {
                int numb2 = Convert.ToInt32(line);
                Console.WriteLine("Исходное число " + numb2);
                if (determine != 0)
                {
                    double detresult = (double)numb2 / determine;
                    results.WriteLine($"{numb2,12}  | {detresult,10}");
                    Console.WriteLine("Результат деления " + detresult);
                }
                else
                {
                    results.WriteLine($"{numb2,12} | Деление на ноль");
                    Console.WriteLine("Ошибка: деление на ноль");

                }
            }
        }
    }
}