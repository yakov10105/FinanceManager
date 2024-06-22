namespace FinanceManager.Services;

public interface ICalculationsService
{
    Task<IEnumerable<string>> SearchTransactionDescriptionsAsync(string prefix,int userId);
    Task CheckBudgetAlertsAsync(int userId);
    Task<List<InvestmentAnalysisResult>> AnalyzeInvestmentPerformanceAsync(int userId);
    Task<List<SavingsGoalOptimizationResult>> OptimizeSavingsGoalsAsync(int userId, decimal availableFunds);
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

    public async Task<List<InvestmentAnalysisResult>> AnalyzeInvestmentPerformanceAsync(int userId)
    {
        var investments = await investmentRepository.GetInvetmentsByUserIdAsync(userId);
        List<InvestmentAnalysisResult> analysisResults = [];

        foreach (var investment in investments)
        {
            // Calculate historical performance (example: annualized return)
            decimal historicalPerformance = CalculateHistoricalPerformance(investment);

            // Predict future performance using dynamic programming
            decimal predictedFuturePerformance = PredictFuturePerformance(investment);

            // Add analysis result
            analysisResults.Add(new InvestmentAnalysisResult
            {
                InvestmentId = investment.Id,
                InvestmentType = investment.Type,
                HistoricalPerformance = historicalPerformance,
                PredictedFuturePerformance = predictedFuturePerformance
            });
        }

        return [..analysisResults];

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
        var transactions = (await transactionRepository.GetTransactionsByUserIdAsync(userId)).ToList();

        // Build the transaction graph
        var transactionGraph = new TransactionGraph();
        foreach (var transaction in transactions)
        {
            transactionGraph.AddTransaction(transaction);
        }

        // Detect anomalies using clustering ,graph traversal and DFS
        var suspiciousTransactions = new List<Transaction>();

        CheckGraphTraversal(transactions, suspiciousTransactions);

        CheckKmeans(transactions, suspiciousTransactions);

        CheckDFS(transactionGraph, suspiciousTransactions);

        return suspiciousTransactions;

    }
    public async Task<List<SavingsGoalOptimizationResult>> OptimizeSavingsGoalsAsync(int userId, decimal availableFunds)
    {
        var savingsGoals = await savingsGoalRepository.GetSavingsGoalByUserIdAsync(userId);

        int n = savingsGoals.Count();
        decimal[] dp = new decimal[(int)availableFunds + 1];
        int[,] keep = new int[n + 1, (int)availableFunds + 1];

        for (int i = 1; i <= n; i++)
        {
            var goal = savingsGoals.ElementAt(i - 1);
            decimal remainingAmount = goal.TargetAmount - goal.CurrentAmount;
            int cost = (int)remainingAmount; // Cost in terms of funds required
            int value = goal.Priority; // Value is based on priority

            for (int w = (int)availableFunds; w >= cost; w--)
            {
                if (dp[w - cost] + value > dp[w])
                {
                    dp[w] = dp[w - cost] + value;
                    keep[i, w] = 1;
                }
            }
        }

        List<SavingsGoalOptimizationResult> results = [];
        for (int i = n, w = (int)availableFunds; i > 0; i--)
        {
            if (keep[i, w] == 1)
            {
                var goal = savingsGoals.ElementAt(i - 1);
                decimal remainingAmount = goal.TargetAmount - goal.CurrentAmount;
                int cost = (int)remainingAmount;

                results.Add(new SavingsGoalOptimizationResult
                {
                    SavingsGoalId = goal.Id,
                    GoalName = goal.GoalName,
                    AllocatedAmount = remainingAmount
                });

                w -= cost;
            }
        }

        return [.. results];

    }
    public async Task<IEnumerable<string>> SearchTransactionDescriptionsAsync(string prefix,int userId)
    {
        await PopulateTransactionTrieAsync(userId);
        return _transactionTrie.Search(prefix);
    }


    private static decimal CalculateHistoricalPerformance(Investment investment)
    {
        // Placeholder: Fetch historical data (for simplicity, using fixed values here)
        // In a real scenario, this data would come from a historical data source or repository
        decimal beginningValue = investment.Amount;
        decimal endingValue = beginningValue * 1.2m; // Assuming a 20% increase for example
        DateTime startDate = investment.Date;
        DateTime endDate = DateTime.Now;

        // Calculate the number of years the investment was held
        int numberOfYears = (endDate.Year - startDate.Year);
        if (numberOfYears == 0)
        {
            numberOfYears = 1; // To avoid division by zero if investment was held for less than a year
        }

        // Calculate the annualized return
        decimal annualizedReturn = (decimal)Math.Pow((double)(endingValue / beginningValue), 1.0 / numberOfYears) - 1;

        return annualizedReturn;
    }
    private static decimal PredictFuturePerformance(Investment investment)
    {
        // Placeholder: Fetch historical data (for simplicity, using fixed values here)
        // In a real scenario, this data would come from a historical data source or repository
        decimal beginningValue = investment.Amount;
        decimal endingValue = beginningValue * 1.2m; // Assuming a 20% increase for example
        DateTime startDate = investment.Date;
        DateTime endDate = DateTime.Now;

        // Calculate the number of years the investment was held
        int numberOfYears = (endDate.Year - startDate.Year);
        if (numberOfYears == 0)
        {
            numberOfYears = 1; // To avoid division by zero if investment was held for less than a year
        }

        // Calculate the average annual growth rate
        decimal annualGrowthRate = (decimal)Math.Pow((double)(endingValue / beginningValue), 1.0 / numberOfYears) - 1;

        // Predict future performance based on the average annual growth rate
        // Let's predict the value 1 year into the future for simplicity
        decimal futurePerformance = investment.Amount * (1 + annualGrowthRate);

        return futurePerformance;
    }

    private static void CheckDFS(TransactionGraph transactionGraph, List<Transaction> suspiciousTransactions)
    {
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
    }
    private static void CheckKmeans(List<Transaction> transactions, List<Transaction> suspiciousTransactions)
    {
        // Prepare data for clustering (e.g., using Amount and Date as features)
        var data = transactions.Select(t => new double[] { (double)t.Amount, t.Date.ToOADate() }).ToArray();

        // Perform K-Means clustering
        var (clusters, centroids) = KMeans.Cluster(data, k: 3);

        // Identify outliers based on cluster assignments and distances to centroids
        for (int i = 0; i < transactions.Count(); i++)
        {
            var transaction = transactions[i];
            var cluster = clusters[i];
            var distance = KMeans.EuclideanDistance(data[i], centroids[cluster]);

            // Define a threshold for outliers (e.g., transactions far from the centroid)
            double threshold = 2.0; // Example threshold, can be adjusted

            if (distance > threshold)
            {
                suspiciousTransactions.Add(transaction);
            }
        }
    }
    private static void CheckGraphTraversal(List<Transaction> transactions, List<Transaction> suspiciousTransactions)
    {
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
    }
    private async Task PopulateTransactionTrieAsync(int userId)
    {
        var transactions = await transactionRepository.GetTransactionsByUserIdAsync(userId);
        foreach (var transaction in transactions)
        {
            _transactionTrie.Insert(transaction.Description);
        }
    }
    private async Task PopulateBudgetHeapAsync(int userId)
    {
        var budgets = await budgetRepository.GetBudgetsByUserIdAsync(userId);
        foreach (var budget in budgets)
        {
            _budgetHeap.Insert(budget);
        }
    }
}
