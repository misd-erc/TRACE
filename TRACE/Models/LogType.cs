using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class LogType
{
    public int LogTypeId { get; set; }

    public string TypeOfLog { get; set; } = null!;

    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
}
