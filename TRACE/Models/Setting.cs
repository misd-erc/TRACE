using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class Setting
{
    public long SettingId { get; set; }

    public string? SettingName { get; set; }

    public string? SettingValue { get; set; }
}
