namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalEarningsObtainedMonthDto
{
    public int Months { get; set; }
    public double TotalTlAmount { get; set; }
    public double TotalCurrencyAmount { get; set; }
}