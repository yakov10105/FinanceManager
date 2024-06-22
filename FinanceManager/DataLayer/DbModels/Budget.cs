namespace FinanceManager.DataLayer.DbModels
{
    public class Budget : IComparable<Budget>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int CompareTo(Budget? other)
        {
            if (other == null) return 1;
            return this.Amount.CompareTo(other.Amount);
        }
    }
}
