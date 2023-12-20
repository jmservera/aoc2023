
public abstract class Gate
{
    static long lowCounter, highCounter;
    public string Name { get; set; }

    public virtual bool State { get; }

    public List<string> Destinations { get; protected set; }
    public static long LowCounter { get => lowCounter; }
    public static long HighCounter { get => highCounter; }


    public Gate(string Name, List<string> destinations)
    {
        this.Name = Name;
        this.Destinations = destinations;
    }

    public IEnumerable<(string, bool, string)> Operation(bool isHigh, string origin = "")
    {
        if (isHigh)
        {
            highCounter++;
        }
        else
        {
            lowCounter++;
        }
        return ExecOperation(isHigh, origin).Select(x => (x.Item1, x.Item2, Name));
    }

    protected abstract IEnumerable<(string, bool)> ExecOperation(bool isHigh, string input = "");

    public static void ResetCounters()
    {
        lowCounter = 0;
        highCounter = 0;
    }
}

public class Test : Gate
{
    public Test(string name, List<string> destinations) : base(name, destinations)
    {
    }

    protected override (string, bool)[] ExecOperation(bool isHigh, string input = "")
    {
        return Array.Empty<(string, bool)>();
    }

    public static Test Instance { get; } = new Test("test", new List<string>());
}

public class FlipFlop : Gate
{
    public override bool State => state;
    bool state = false;
    public FlipFlop(string name, List<string> destinations) : base(name, destinations)
    {
    }

    protected override IEnumerable<(string, bool)> ExecOperation(bool isHigh, string input = "")
    {
        if (isHigh)
        {
            //ignore
            return Array.Empty<(string, bool)>();
        }
        else
        {
            state = !state;
            return Destinations.Zip(Enumerable.Repeat(state, Destinations.Count));
        }
    }
}

public class Conjunction : Gate
{
    public override bool State => states.Values.Any(c => !c);
    private readonly Dictionary<string, bool> states = new();
    public Conjunction(string name, List<string> destinations) : base(name, destinations)
    {
    }

    public void AddInput(string input)
    {
        if (!states.ContainsKey(input))
        {
            states.Add(input, false);
        }
    }

    protected override IEnumerable<(string, bool)> ExecOperation(bool isHigh, string input = "")
    {
        states[input] = isHigh;
        var result = states.Values.Any(c => !c);

        return Destinations.Zip(Enumerable.Repeat(result, Destinations.Count));
    }
}

public class Broadcast : Gate
{
    public Broadcast(string name, List<string> destinations) : base(name, destinations)
    {
    }

    protected override IEnumerable<(string, bool)> ExecOperation(bool isHigh, string input = "")
    {
        return Destinations.Zip(Enumerable.Repeat(isHigh, Destinations.Count));
    }
}