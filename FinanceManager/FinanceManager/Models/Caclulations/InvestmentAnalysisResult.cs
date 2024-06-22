namespace FinanceManager.Models.Caclulations;

public class InvestmentAnalysisResult
{
    public int InvestmentId { get; set; }
    public string InvestmentType { get; set; }
    public decimal HistoricalPerformance { get; set; }
    public decimal PredictedFuturePerformance { get; set; }
}
