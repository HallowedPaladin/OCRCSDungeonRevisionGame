﻿using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Preference
{
    public int UserId { get; set; }

    public string? Locale { get; set; }

    public virtual User User { get; set; } = null!;
}
