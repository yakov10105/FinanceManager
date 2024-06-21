namespace FinanceManager.DataLayer.Repositories;

public interface IBudgetRepository
{
    Task<Budget?> GetBudgetByIdAsync(int id);
    Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int id);
    Task AddBudgetAsync(Budget budget);
    Task DeleteBudgetAsync(int id);
    Task UpdateBudgetAsync(Budget budget);
}
public class BudgetRepository(FinanceDbContext dbContext) : IBudgetRepository
{
    public async Task AddBudgetAsync(Budget budget)
    {
        await dbContext.Budgets.AddAsync(budget);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteBudgetAsync(int id)
    {
        var budget = await dbContext.Budgets.FindAsync(id);
        if (budget != null)
        {
            dbContext.Budgets.Remove(budget);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Budget?> GetBudgetByIdAsync(int id)
    {
        return await dbContext.Budgets.FindAsync(id);
    }

    public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(int id)
    {
        return await dbContext.Budgets.Where(b => b.UserId == id).ToListAsync();
    }

    public async Task UpdateBudgetAsync(Budget budget)
    {
        dbContext.Budgets.Update(budget);
        await dbContext.SaveChangesAsync();
    }
}
