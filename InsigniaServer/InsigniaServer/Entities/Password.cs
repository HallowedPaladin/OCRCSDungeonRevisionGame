using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class Password
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
