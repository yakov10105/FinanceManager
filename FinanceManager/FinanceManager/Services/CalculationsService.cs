namespace FinanceManager.Services;

public interface ICalculationsService
{
    IEnumerable<string> SearchTransactionDescriptionsAsync(string prefix);
    Task CheckBudgetAlertsAsync(int userId);
    Task<InvestmentAnalysisResult> AnalyzeInvestmentPerformanceAsync(int userId);
    Task<SavingsGoalOptimizationResult> OptimizeSavingsGoalsAsync(int userId, decimal availableFunds);
    Task<IEnumerable<Transaction>> DetectFraudulentTransactionsAsync(int userId);
}


public class CalculationsService(
    ILogger<CalculationsService> logger,
    ITransactionRepository transactionRepository,
    IBudgetRepository budgetRepository,
    IInvestmentRepository investmentRepository,
    ISavingsGoalRepository savingsGoalRepository
    ) : ICalculationsService
{
    private readonly Trie _transactionTrie = new();
    private readonly MinHeap<Budget> _budgetHeap = new();

    public async Task<InvestmentAnalysisResult> AnalyzeInvestmentPerformanceAsync(int userId)
    {
        var investments = await investmentRepository.GetInvetmentsByUserIdAsync(userId);
        // Implement dynamic programming analysis for investment performance
        // ...
        return new InvestmentAnalysisResult { /* Populate with analysis data */ };

    }

    public  async Task CheckBudgetAlertsAsync(int userId)
    {
        await PopulateBudgetHeapAsync(userId);

        // Check each budget in the heap
        while (!_budgetHeap.IsEmpty())
        {
            var budget = _budgetHeap.ExtractMin();
            var transactions = await transactionRepository.GetTransactionsByUserIdAsync(userId);
            var totalSpent = transactions.Where(t => t.Category == budget.Category).Sum(t => t.Amount);

            if (totalSpent >= budget.Amount)
            {
                // Send alert
                Console.WriteLine($"Budget alert: You have exceeded your budget for {budget.Category}");
            }
            else if (totalSpent >= 0.9m * budget.Amount)
            {
                // Send alert
                Console.WriteLine($"Budget alert: You are close to exceeding your budget for {budget.Category}");
            }
        }
    }

    public async Task<IEnumerable<Transaction>> DetectFraudulentTransactionsAsync(int userId)
    {
        var transactions = await transactionRepository.GetTransactionsByUserIdAsync(userId);

        // Build the transaction graph
        var transactionGraph = new TransactionGraph();
        foreach (var transaction in transactions)
        {
            transactionGraph.AddTransaction(transaction);
        }

        // Detect anomalies using clustering and graph traversal
        var suspiciousTransactions = new List<Transaction>();

        // Example: Detect anomalies based on transaction amount and frequency
        var transactionAmounts = transactions.Select(t => t.Amount).ToArray();
        var mean = transactionAmounts.Average();
        var stdDev = Math.Sqrt(transactionAmounts.Select(a => Math.Pow((double)(a - mean), 2)).Average());

        // Transactions that deviate significantly from the mean are considered suspicious
        foreach (var transaction in transactions)
        {
            if (Math.Abs((double)(transaction.Amount - mean)) > 2 * stdDev)
            {
                suspiciousTransactions.Add(transaction);
            }
        }

        // Additional anomaly detection using graph traversal (e.g., detecting cycles)
        var visited = new HashSet<int>();
        var stack = new Stack<int>();


        void Dfs(int node, int parent)
        {
            visited.Add(node);
            stack.Push(node);

            foreach (var neighbor in transactionGraph.GetTransactions(node))
            {
                if (!visited.Contains(neighbor.UserId))
                {
                    Dfs(neighbor.UserId, node);
                }
                else if (neighbor.UserId != parent)
                {
                    // Cycle detected
                    suspiciousTransactions.Add(neighbor);
                }
            }

            stack.Pop();
        }

        foreach (var node in transactionGraph.AdjacencyList.Keys)
        {
            if (!visited.Contains(node))
            {
                Dfs(node, -1);
            }
        }

        return suspiciousTransactions;

    }

    public async Task<SavingsGoalOptimizationResult> OptimizeSavingsGoalsAsync(int userId, decimal availableFunds)
    {
        var savingsGoals = await savingsGoalRepository.GetSavingsGoalByUserIdAsync(userId);
        // Implement the knapsack problem algorithm for optimal fund allocation
        // ...
        return new SavingsGoalOptimizationResult { /* Populate with optimization data */ };

    }

    public IEnumerable<string> SearchTransactionDescriptionsAsync(string prefix)
    {
        return _transactionTrie.Search(prefix);
    }

    private async Task PopulateTransactionTrieAsync(int userId)
    {
        var transactions = await transactionRepository.GetTransactionsByUserIdAsync(userId);
        foreach (var transaction in transactions)
        {
            _transactionTrie.Insert(transaction.Description);
        }
    }

    // Populate the Min-Heap with budgets
    private async Task PopulateBudgetHeapAsync(int userId)
    {
        var budgets = await budgetRepository.GetBudgetsByUserIdAsync(userId);
        foreach (var budget in budgets)
        {
            _budgetHeap.Insert(budget);
        }
    }
}
