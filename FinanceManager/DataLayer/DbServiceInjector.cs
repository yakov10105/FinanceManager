namespace FinanceManager.DataLayer;

public static class DbServiceInjector
{
    public static void InjectDbDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FinanceDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("FinanceDbConnectionString"));
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<ISavingsGoalRepository, SavingsGoalRepository>();
        builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();
    }
}
