using System;
using System.Collections.Generic;

namespace E_BangEmailWorker.Database;

public partial class ProcedureLog
{
    public int Id { get; set; }

    public string ProcedureName { get; set; } = null!;

    public DateTime ExecutionTime { get; set; }

    public string Status { get; set; } = null!;

    public string? Message { get; set; }
}
