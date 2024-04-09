using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int SubjectId { get; set; }

    public string AssignmentName { get; set; } = null!;

    public string? AssignmentDescription { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<AssignmentScore> AssignmentScores { get; set; } = new List<AssignmentScore>();

    public virtual Subject Subject { get; set; } = null!;
}
