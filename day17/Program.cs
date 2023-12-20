var paths = File.ReadAllLines("test.txt").Select(x => x.Select(n => int.Parse(n.ToString())).ToArray()).ToArray();
