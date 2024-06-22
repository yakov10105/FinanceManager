namespace FinanceManager.DataLayer.DbModels
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string GoalName { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public int Priority { get; set; }
    }
}
