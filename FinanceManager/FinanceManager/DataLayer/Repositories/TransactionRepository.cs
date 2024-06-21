namespace FinanceManager.DataLayer.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetTransactionByIdAsync(int id);
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int id);
    Task DeleteTransactionAsync(int id);
    Task AddTransactionAsync(Transaction transaction);
    Task UpdateTransactionAsync(Transaction transaction);
}
public class TransactionRepository(FinanceDbContext dbContext) : ITransactionRepository
{
    public async Task AddTransactionAsync(Transaction transaction)
    {
        await dbContext.Transactions.AddAsync(transaction);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id)
    {
        var budget = dbContext.Transactions.Find(id);
        if (budget != null)
        {
            dbContext.Transactions.Remove(budget);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int id)
    {
        return await dbContext.Transactions.FindAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int id)
    {
        return await dbContext.Transactions.Where(b => b.UserId == id).ToListAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        dbContext.Transactions.Update(transaction);
        await dbContext.SaveChangesAsync();
    }
}
