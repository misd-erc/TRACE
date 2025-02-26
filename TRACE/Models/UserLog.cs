using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class UserLog
{
    public long UserLogId { get; set; }

    public string UserId { get; set; } = null!;

    public string LogMessage { get; set; } = null!;

    public DateTime LogDatetime { get; set; }

    public int LogTypeId { get; set; }

    public virtual LogType LogType { get; set; } = null!;
}
