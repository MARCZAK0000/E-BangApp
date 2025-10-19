using System;
using System.Collections.Generic;

namespace E_BangEmailWorker.Database;

public partial class EmailSetting
{
    public int Id { get; set; }

    public string EmailName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string SmptHost { get; set; } = null!;

    public int Port { get; set; }

    public string Salt { get; set; } = null!;

    public DateTime LastUpdateTime { get; set; }
}
