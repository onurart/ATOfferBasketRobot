namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOrderedSalesCompareDayDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public Decimal RobotEarnings { get; set; }
    public Decimal EarningsWithoutRobot { get; set; }
    public Decimal Ratio { get; set; }

}