namespace ATBasketRobotServer.Domain.Dtos.Report;
public class TotalOrderedSalesCompareMounthDto
{
    public int Month { get; set; }
    public Decimal RobotEarnings { get; set; }
    public Decimal EarningsWithoutRobot { get; set; }
    public Decimal Ratio { get; set; }
}