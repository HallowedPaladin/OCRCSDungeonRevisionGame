using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class UserLogon
{
    public int UserId { get; set; }

    public DateTime LastLogonDate { get; set; }

    public int LogonAttempts { get; set; }

    public int IsLocked { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
