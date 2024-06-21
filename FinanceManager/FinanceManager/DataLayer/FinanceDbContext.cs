namespace FinanceManager.DataLayer;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Investment> Investments { get; set; }
    public DbSet<SavingsGoal> SavingsGoals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Budgets)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Investments)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.SavingsGoals)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);
    }
}
