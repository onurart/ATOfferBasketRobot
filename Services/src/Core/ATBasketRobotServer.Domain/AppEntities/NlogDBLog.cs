using System.ComponentModel.DataAnnotations;
namespace ATBasketRobotServer.Domain.AppEntities;
public class NlogDBLog
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    [StringLength(10)]
    public string Level { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Exception { get; set; }
    [StringLength(255)]
    public string Logger { get; set; }
    [StringLength(255)]
    public string Url { get; set; }

}