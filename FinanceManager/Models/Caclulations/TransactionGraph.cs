namespace FinanceManager.Models.Caclulations;

public class TransactionGraph
{
    public Dictionary<int, List<Transaction>> AdjacencyList { get; private set; }

    public TransactionGraph()
    {
        AdjacencyList = [];
    }

    public void AddTransaction(Transaction transaction)
    {
        if (!AdjacencyList.TryGetValue(transaction.UserId, out List<Transaction>? value))
        {
            value = new List<Transaction>();
            AdjacencyList[transaction.UserId] = value;
        }

        value.Add(transaction);
    }

    public List<Transaction> GetTransactions(int userId)
    {
        return AdjacencyList.TryGetValue(userId, out List<Transaction>? value) ? value : new List<Transaction>();
    }
}
