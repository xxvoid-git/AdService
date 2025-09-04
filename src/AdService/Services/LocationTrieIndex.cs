namespace AdService.Services;

public class LocationTrieIndex : ILocationIndex
{
    private class Node
    {
        public Dictionary<string, Node> Children { get; } = new();
        public HashSet<string> Platforms { get; } = new();
    }

    private readonly Node _root = new();

    public void Load(IEnumerable<string> lines)
    {
        lock (_root)
        {
            _root.Children.Clear();
            _root.Platforms.Clear();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || !line.Contains(":")) continue;
                var parts = line.Split(":", 2);
                var platform = parts[0].Trim();
                var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var loc in locations)
                {
                    var path = loc.Trim().Split("/", StringSplitOptions.RemoveEmptyEntries);
                    var node = _root;
                    foreach (var segment in path)
                    {
                        if (!node.Children.TryGetValue(segment, out var next))
                        {
                            next = new Node();
                            node.Children[segment] = next;
                        }
                        node = next;
                    }
                    node.Platforms.Add(platform);
                }
            }
        }
    }

    public IEnumerable<string> Search(string location)
    {
        var result = new HashSet<string>();
        var path = location.Trim().Split("/", StringSplitOptions.RemoveEmptyEntries);

        var node = _root;
        result.UnionWith(node.Platforms);

        foreach (var segment in path)
        {
            if (!node.Children.TryGetValue(segment, out node!)) break;
            result.UnionWith(node.Platforms);
        }

        return result;
    }
}
