namespace FinanceManager.DataLayer.DbModels;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateCreated { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Budget> Budgets { get; set; }
    public ICollection<Investment> Investments { get; set; }
    public ICollection<SavingsGoal> SavingsGoals { get; set; }
}
