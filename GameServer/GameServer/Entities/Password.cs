using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Password
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
