using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class UserTokenCache
{
    public int UserTokenCacheId { get; set; }

    public string? WebUserUniqueId { get; set; }

    public byte[]? CacheBits { get; set; }

    public DateTime LastWrite { get; set; }
}
