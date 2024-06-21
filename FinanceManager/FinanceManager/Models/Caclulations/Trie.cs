namespace FinanceManager.Models.Caclulations;

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public TrieNode()
    {
        Children = [];
        IsEndOfWord = false;
    }
}

public class Trie
{
    private readonly TrieNode _root;

    public Trie()
    {
        _root = new TrieNode();
    }

    public void Insert(string word)
    {
        var node = _root;
        foreach (var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out TrieNode? value))
            {
                value = new TrieNode();
                node.Children[ch] = value;
            }
            node = value;
        }
        node.IsEndOfWord = true;
    }

    public IEnumerable<string> Search(string prefix)
    {
        var node = _root;
        foreach (var ch in prefix)
        {
            if (!node.Children.TryGetValue(ch, out TrieNode? value))
            {
                return Enumerable.Empty<string>();
            }
            node = value;
        }
        return SearchAllWords(node, prefix);
    }

    private IEnumerable<string> SearchAllWords(TrieNode node, string prefix)
    {
        var results = new List<string>();
        if (node.IsEndOfWord)
        {
            results.Add(prefix);
        }
        foreach (var child in node.Children)
        {
            results.AddRange(SearchAllWords(child.Value, prefix + child.Key));
        }
        return results;
    }
}
