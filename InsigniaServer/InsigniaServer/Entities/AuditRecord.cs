using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class AuditRecord
{
    public int UserId { get; set; }

    public DateTime ActivityTime { get; set; }

    public string ActivityType { get; set; } = null!;

    public string? ActivityDetails { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
