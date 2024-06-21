namespace FinanceManager.DataLayer.Repositories;

public interface ISavingsGoalRepository
{
    Task<SavingsGoal?> GetSavingsGoalByIdAsync(int id);
    Task<IEnumerable<SavingsGoal>> GetSavingsGoalByUserIdAsync(int id);
    Task AddSavingsGoalAsync(SavingsGoal goal);
    Task DeleteSavingsGoalAsync(int id);
    Task UpdateSavingsGoalAsync(SavingsGoal goal);

}
public class SavingsGoalRepository(FinanceDbContext dbContext) : ISavingsGoalRepository
{
    public async Task AddSavingsGoalAsync(SavingsGoal goal)
    {
        await dbContext.SavingsGoals.AddAsync(goal);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteSavingsGoalAsync(int id)
    {
        var goal = await dbContext.SavingsGoals.FindAsync(id);
        if (goal != null)
        {
            dbContext.SavingsGoals.Remove(goal);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<SavingsGoal?> GetSavingsGoalByIdAsync(int id)
        => await dbContext.SavingsGoals.FindAsync(id);

    public async Task<IEnumerable<SavingsGoal>> GetSavingsGoalByUserIdAsync(int id)
        => await dbContext.SavingsGoals.Where(g => g.UserId == id).ToListAsync();

    public Task UpdateSavingsGoalAsync(SavingsGoal goal)
    {
        throw new NotImplementedException();
    }
}
