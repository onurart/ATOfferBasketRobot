namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalEarningsObtainedDayDto
{
    public int Months { get; set; }
    public int Days { get; set; }
    public double TotalTlAmount { get; set; }
    public double TotalCurrencyAmount { get; set; }
}