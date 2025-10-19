using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangEmailWorker.Database;
[Table("Email", Schema = "Current")]
public partial class Email
{
    public int EmailId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public bool IsSend { get; set; }

    public DateTime SendTime { get; set; }

    public DateTime CreatedTime { get; set; }
}
