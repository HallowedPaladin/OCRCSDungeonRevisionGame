using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class SubjectTeacher
{
    public int UserId { get; set; }

    public int SubjectId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Subject Subject { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
