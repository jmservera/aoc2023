internal class Program
{
    static Dictionary<string, Gate> CreateGates(string[] lines)
    {
        Gate.ResetCounters();
        var gates = lines.Select(line => line.Split(" -> "))
            .Select(line => new { Name = line[0], Destinations = line[1].Split(", ").ToList() })
            .Select(line =>
            {
                var operation = line.Name.StartsWith("%") ? "%" : line.Name.StartsWith("&") ? "&" : "";

                var name = operation == "" ? line.Name : line.Name.Replace(operation, "");
                return operation switch
                {
                    "%" => (Gate)new FlipFlop(name, line.Destinations),
                    "&" => new Conjunction(name, line.Destinations),
                    _ => new Broadcast(name, line.Destinations),
                };
            })
            .ToDictionary(gate => gate.Name, gate => gate);
        foreach (var gate in gates.Values)
        {
            foreach (var destination in gate.Destinations)
            {
                if (gates.ContainsKey(destination))
                {
                    var destGate = gates[destination];
                    if (destGate is Conjunction conjunction)
                    {
                        conjunction.AddInput(gate.Name);
                    }
                }
            }
        }
        return gates;
    }

    static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long lcm(long a, long b)
    {
        return a / gcf(a, b) * b;
    }
    private static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        Dictionary<string, Gate> gates = CreateGates(lines);

        //EXERCISE 1

        for (long n = 0; n < 1000; n++)
        {
            var next = gates["broadcaster"].Operation(false, "broadcaster");
            while (next.Count() > 0)
            {
                var newNext = new List<(string, bool, string)>();

                foreach (var (name, isHigh, origin) in next)
                {
                    if (gates.ContainsKey(name))
                    {
                        var gate = gates[name];

                        var result = gate.Operation(isHigh, origin);
                        newNext.AddRange(result);
                    }
                    else
                    {
                        Test.Instance.Operation(isHigh, origin);
                    }
                }
                next = newNext.ToArray();
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Low: {Gate.LowCounter}, High: {Gate.HighCounter}");
        Console.WriteLine($"{Gate.LowCounter * Gate.HighCounter}");


        // EXERCISE 2

        gates = CreateGates(lines);

        var secondLevelRx = gates.Where(gate => gate.Value.Destinations.Any(d =>
        {
            if (gates.TryGetValue(d, out Gate? g))
            {
                return g.Destinations.Contains("rx");
            }
            return false;
        })).Select(gate => KeyValuePair.Create(gate.Key, (last: 0L, period: 0L, validPeriods: 0)))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


        bool rx = true;
        long i = 0;
        while (rx)
        {
            var next = gates["broadcaster"].Operation(false, "broadcaster");
            while (next.Any() && rx)
            {
                var newNext = new List<(string, bool, string)>();

                foreach (var (name, isHigh, origin) in next)
                {
                    if (gates.TryGetValue(name, out var gate))
                    {
                        var result = gate.Operation(isHigh, origin);
                        newNext.AddRange(result);
                        // calculate periods for each conjunction gate that is two levels below rx
                        if (secondLevelRx.ContainsKey(name) && !isHigh)
                        {
                            var (last, period, validPeriods) = secondLevelRx[name];
                            if (last == 0)
                            {
                                secondLevelRx[name] = (i, 0, 0);
                            }
                            else
                            {
                                if (i - last == period)
                                {
                                    secondLevelRx[name] = (i, period, validPeriods + 1);
                                }
                                else
                                {
                                    secondLevelRx[name] = (i, i - last, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        Test.Instance.Operation(isHigh, origin);
                        if (name == "rx" && !isHigh)
                        {
                            Console.WriteLine($"rx: {i}");
                            rx = false;
                            break;
                        }
                    }
                }
                next = newNext;
            }

            if (i % 1000000 == 0)
                Console.Write(".");

            // check for valid periods 4 times in a row of each conjunction gate that has rx as indirect destination and has a low value
            if (secondLevelRx.All(r => r.Value.period > 0 && r.Value.validPeriods > 4))
            {
                Console.WriteLine($"Periods: {string.Join(", ", secondLevelRx.Select(r => r.Value.period).ToArray())}");
                // the LCM of all the periods is the solution
                Console.WriteLine($"LCM: {secondLevelRx.Select(r => r.Value.period).Aggregate((a, b) => lcm(a, b))}");
                break;
            }

            i++;
        }
    }
}