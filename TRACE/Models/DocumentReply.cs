using System;
using System.Collections.Generic;

namespace TRACE.Models;

public partial class DocumentReply
{
    public long DocumentReplyId { get; set; }

    public long? DocumentId { get; set; }

    public long? RepliedDocumentId { get; set; }

    public virtual Document? Document { get; set; }

    public virtual Document? RepliedDocument { get; set; }
}
