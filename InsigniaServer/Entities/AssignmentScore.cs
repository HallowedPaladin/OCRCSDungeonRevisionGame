using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class AssignmentScore
{
    public int ScoreId { get; set; }

    public int UserId { get; set; }

    public int AssignmentId { get; set; }

    public DateTime ScoreDateTime { get; set; }

    public int Score { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
