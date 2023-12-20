using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

public abstract class Gate
{
    static long lowCounter, highCounter;
    public string Name { get; set; }

    public bool Result { get; protected set; }

    public string[] Destinations { get; protected set; }
    public static long LowCounter { get => lowCounter; }
    public static long HighCounter { get => highCounter; }


    public Gate(string Name, string[] destinations)
    {
        this.Name = Name;
        this.Destinations = destinations;
    }

    public (string, bool, string)[] Operation(bool isHigh, string origin = "")
    {
        if (isHigh)
        {
            highCounter++;
        }
        else
        {
            lowCounter++;
        }
        return ExecOperation(isHigh, origin).Select(x => (x.Item1, x.Item2, Name)).ToArray();
    }

    protected abstract (string, bool)[] ExecOperation(bool isHigh, string input = "");
}

public class Test : Gate
{
    public Test(string name, string[] destinations) : base(name, destinations)
    {
    }

    protected override (string, bool)[] ExecOperation(bool isHigh, string input = "")
    {
        return Array.Empty<(string, bool)>();
    }

    public static Test Instance { get; } = new Test("test", Array.Empty<string>());
}

public class FlipFlop : Gate
{
    bool state = false;
    public FlipFlop(string name, string[] destinations) : base(name, destinations)
    {
    }

    protected override (string, bool)[] ExecOperation(bool isHigh, string input = "")
    {
        if (isHigh)
        {
            //ignore
            return Array.Empty<(string, bool)>();
        }
        else
        {
            state = !state;
            return Destinations.Zip(Enumerable.Repeat(state, Destinations.Length))
                    .ToArray();
        }
    }
}

public class Conjunction : Gate
{
    Dictionary<string, bool> states = new Dictionary<string, bool>();
    public Conjunction(string name, string[] destinations) : base(name, destinations)
    {
    }

    public void AddInput(string input)
    {
        if (!states.ContainsKey(input))
        {
            states.Add(input, false);
        }
    }

    protected override (string, bool)[] ExecOperation(bool isHigh, string input = "")
    {
        states[input] = isHigh;
        var result = !states.Values.Aggregate(true, (acc, val) => acc && val);

        return Destinations.Zip(Enumerable.Repeat(result, Destinations.Length))
                    .ToArray();
    }
}

public class Broadcast : Gate
{
    public Broadcast(string name, string[] destinations) : base(name, destinations)
    {
    }

    protected override (string, bool)[] ExecOperation(bool isHigh, string input = "")
    {
        return Destinations.Zip(Enumerable.Repeat(isHigh, Destinations.Length))
                    .ToArray();
    }
}