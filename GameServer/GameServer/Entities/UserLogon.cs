using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class UserLogon
{
    public int UserId { get; set; }

    public DateTime LastLogonDate { get; set; }

    public int LogonAttempts { get; set; }

    public sbyte IsLocked { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
