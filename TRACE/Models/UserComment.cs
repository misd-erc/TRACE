using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class UserComment
{
    public long UserCommentId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime DatetimeCreated { get; set; }

    public string Comment { get; set; } = null!;

    public long DocumentId { get; set; }

    public virtual Document Document { get; set; } = null!;
}
