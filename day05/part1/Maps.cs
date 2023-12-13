using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

namespace day5.part1;

public class Maps
{
    public static void Transform(ref long[] input, IEnumerator<string> linesEnum)
    {
        List<long[]> origToDest = new();
        List<long> result = new();
        linesEnum.MoveNext();
        while (linesEnum.Current.Length > 0)
        {
            origToDest.Add(linesEnum.Current.Split().Select(x => long.Parse(x)).ToArray());
            if (!linesEnum.MoveNext())
                break;
        }
        var arr = input;
        Parallel.For(0, input.Length, (l) =>
        {
            foreach (var soil in origToDest)
            {
                if (soil[1] <= arr[l] && arr[l] < soil[1] + soil[2])
                {
                    arr[l] = soil[0] + (arr[l] - soil[1]);
                    break;
                }
            }
        });
    }
    public static long LowestLocation(string fileName)
    {

        var lines = File.ReadAllLines(fileName);
        var linesEnum = lines.AsEnumerable().GetEnumerator();
        linesEnum.MoveNext();
        long[] result = linesEnum.Current
                        .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries)[1..]
                        .Select(x => long.Parse(x)).ToArray();
        linesEnum.MoveNext();

        for (int i = 0; i < 7; i++)
        {
            linesEnum.MoveNext();
            Transform(ref result, linesEnum);
            Console.WriteLine("Phase {0}", i + 1);
        }
        return result.Min();
    }

    public static IEnumerable<long> CreateRange(long start, long count)
    {
        var limit = start + count;

        while (start < limit)
        {
            yield return start;
            start++;
        }
    }

    public static long LowestLocation2(string fileName)
    {

        var lines = File.ReadAllLines(fileName);
        var linesEnum = lines.AsEnumerable().GetEnumerator();
        linesEnum.MoveNext();
        long[][] seeds = linesEnum.Current
                        .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries)[1..]
                        .Select(x => long.Parse(x)).Chunk(2).ToArray();
        linesEnum.MoveNext();

        var length = seeds.Sum(r => r[1]);
        var result = new long[length];
        long index = 0;
        foreach (var seed in seeds)
        {
            foreach (var value in CreateRange(seed[0], seed[1]))
            {
                result[index++] = value;
            }
        }

        for (int i = 0; i < 7; i++)
        {
            linesEnum.MoveNext();
            Transform(ref result, linesEnum);
            Console.WriteLine("Phase {0}", i + 1);
        }

        return result.Min();
    }
}