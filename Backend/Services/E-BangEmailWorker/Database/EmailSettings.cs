using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EmailSettings", Schema = "Settings")]
public class EmailSettings
{
    [Key]
    public int Id { get; set; }
    public string EmailName { get; set; }
    public string Password { get; set; }
    public string SmptHost { get; set; }
    public int Port { get; set; }
    public required string Salt { get; set; }
    [DataType(DataType.Date)]
    public DateTime LastUpdateTime { get; set; } = DateTime.Now;
}