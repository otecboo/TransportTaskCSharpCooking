using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {   
        // Чтение входных данных
        string[] inputLines = File.ReadAllLines("in.txt");
        var firstLine = inputLines[0].Split(' ').Select(int.Parse).ToArray();
        int N = firstLine[0]; // Количество поставщиков
        int M = firstLine[1]; // Количество потребителей

        int[] supplies = inputLines[1].Split(' ').Select(int.Parse).ToArray();
        int[] demands = inputLines[2].Split(' ').Select(int.Parse).ToArray();
        int[,] costs = new int[N, M];

        for (int i = 0; i < N; i++)
        {
            var costLine = inputLines[i + 3].Split(' ').Select(int.Parse).ToArray();
            for (int j = 0; j < M; j++)
            {
                costs[i, j] = costLine[j];
            }
        }

        // Решение транспортной задачи
        int[,] transports = new int[N, M];
        SolveTransportationProblem(supplies, demands, costs, transports, N, M);

        // Подсчет суммарных затрат
        int totalCost = CalculateTotalCost(transports, costs, N, M);

        // Запись результатов в файл
        using (StreamWriter writer = new StreamWriter("out.txt"))
        {
            writer.WriteLine(totalCost);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    writer.Write(transports[i, j] + (j < M - 1 ? " " : ""));
                }
                writer.WriteLine();
            }
        }
    }

    static void SolveTransportationProblem(int[] supplies, int[] demands, int[,] costs, int[,] transports, int N, int M)
    {
        int supplyIndex = 0, demandIndex = 0;

        // Метод Северо-Западного Угла
        while (supplyIndex < N && demandIndex < M)
        {
            int transportAmount = Math.Min(supplies[supplyIndex], demands[demandIndex]);
            transports[supplyIndex, demandIndex] = transportAmount;
            supplies[supplyIndex] -= transportAmount;
            demands[demandIndex] -= transportAmount;

            // Если поставщик исчерпан, переходим к следующему
            if (supplies[supplyIndex] == 0)
            {
                supplyIndex++;
            }

            // Если потребитель исчерпан, переходим к следующему
            if (demands[demandIndex] == 0)
            {
                demandIndex++;
            }
        }
    }

    static int CalculateTotalCost(int[,] transports, int[,] costs, int N, int M)
    {
        int totalCost = 0;

        Parallel.For(0, N, i =>
        {
            for (int j = 0; j < M; j++)
            {
                totalCost += transports[i, j] * costs[i, j];
            }
        });

        return totalCost;
    }
}