using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Чтение данных из файла
        var (supply, demand, costs) = ReadData("in.txt");
        int n = supply.Length; // Количество поставщиков
        int m = demand.Length; // Количество потребителей

        // Начальное 
        var allocation = new int[n, m];
        NorthWestCornerMethod(supply, demand, costs, allocation);

        
        OptimizeTransportation(supply, demand, costs, allocation);

        // Запись результатов в файлe
        WriteResults("out.txt", allocation);
    }

    static (int[] supply, int[] demand, int[,] costs) ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var firstLine = lines[0].Split(' ').Select(int.Parse).ToArray();
        int n = firstLine[0], m = firstLine[1];
        
        var supply = lines[1].Split(' ').Select(int.Parse).ToArray();
        var demand = lines[2].Split(' ').Select(int.Parse).ToArray();
        var costs = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            var costLine = lines[i + 3].Split(' ').Select(int.Parse).ToArray();
            for (int j = 0; j < m; j++)
            {
                costs[i, j] = costLine[j];
            }
        }

        return (supply, demand, costs);
    }

    static void NorthWestCornerMethod(int[] supply, int[] demand, int[,] costs, int[,] allocation)
    {
        int n = supply.Length;
        int m = demand.Length;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (supply[i] == 0 || demand[j] == 0) continue;

                int amount = Math.Min(supply[i], demand[j]);
                allocation[i, j] = amount;
                supply[i] -= amount;
                demand[j] -= amount;
            }
        }
    }

    static void OptimizeTransportation(int[] supply, int[] demand, int[,] costs, int[,] allocation)
    {
        // Здесь можно реализовать метод оптимизации, например, метод потенциалов
        // 
    }
        //TODO: вложенные циклы нехорошо
    static void WriteResults(string filePath, int[,] allocation)
    {
        using (var writer = new StreamWriter(filePath))
        {
            int totalCost = 0;
            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    totalCost += allocation[i, j] * costs[i, j]; // costs должен быть доступен
                }
            }
            writer.WriteLine(totalCost);

            for (int i = 0; i < allocation.GetLength(0); i++)
            {
                for (int j = 0; j < allocation.GetLength(1); j++)
                {
                    writer.Write(allocation[i, j] + " ");
                }
                writer.WriteLine();
            }
        }
    }
}