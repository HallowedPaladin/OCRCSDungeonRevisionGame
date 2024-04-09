using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class AssignmentScore
{
    public int ScoreId { get; set; }

    public int UserId { get; set; }

    public int AssignmentId { get; set; }

    public DateTime Date { get; set; }

    public int Score { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
