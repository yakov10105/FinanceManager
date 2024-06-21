namespace FinanceManager.DataLayer.Repositories;

public interface IInvestmentRepository
{
    Task<Investment?> GetInvetmentByIdAsync(int id);
    Task<IEnumerable<Investment>> GetInvetmentsByUserIdAsync(int id);
    Task AddInvestmentAsync(Investment investment);
    Task UpdateInvestmentAsync(Investment investment);
    Task DeleteInvestmentAsync(int id);
}
public class InvestmentRepository(FinanceDbContext dbContext) : IInvestmentRepository
{
    public async Task AddInvestmentAsync(Investment investment)
    {
        await dbContext.Investments.AddAsync(investment);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteInvestmentAsync(int id)
    {
        var investment = dbContext.Investments.Find(id);
        if (investment != null)
        {
            dbContext.Investments.Remove(investment);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Investment?> GetInvetmentByIdAsync(int id)
        => await dbContext.Investments.FindAsync(id);

    public async Task<IEnumerable<Investment>> GetInvetmentsByUserIdAsync(int id)
        =>await dbContext.Investments.Where(inv=>inv.UserId==id).ToListAsync();

    public async Task UpdateInvestmentAsync(Investment investment)
    {
        dbContext.Investments.Update(investment);
        await dbContext.SaveChangesAsync();
    }
}
