using System.Data;

var lines = File.ReadAllLines("input.txt");

var gates = lines.Select(line => line.Split(" -> "))
    .Select(line => new { Name = line[0], Destinations = line[1].Split(", ") })
    .Select(line =>
    {
        var operation = line.Name.StartsWith("%") ? "%" : line.Name.StartsWith("&") ? "&" : "";

        var name = operation == "" ? line.Name : line.Name.Replace(operation, "");
        switch (operation)
        {
            case "%":
                return (Gate)new FlipFlop(name, line.Destinations);
            case "&":
                return new Conjunction(name, line.Destinations);
            default:
                return new Broadcast(name, line.Destinations);
        }
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

bool rx = true;
for (long i = 0; rx; i++)
{
    var next = gates["broadcaster"].Operation(false, "broadcaster");
    while (next.Length > 0)
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
                if (name == "rx" && !isHigh)
                {
                    Console.WriteLine($"rx: {i}");
                    rx = false;
                    break;
                }
            }
        }
        next = newNext.ToArray();
    }
    if (i % 100000 == 0)
        Console.Write(".");
}

Console.WriteLine();
Console.WriteLine($"Low: {Gate.LowCounter}, High: {Gate.HighCounter}");
Console.WriteLine($"{Gate.LowCounter * Gate.HighCounter}");
