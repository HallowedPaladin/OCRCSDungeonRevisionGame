using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int SubjectId { get; set; }

    public string AssignmentName { get; set; } = null!;

    public string? AssignmentDescription { get; set; }

    public sbyte IsPublished { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<AssignmentQuestionPack> AssignmentQuestionPacks { get; set; } = new List<AssignmentQuestionPack>();

    public virtual ICollection<AssignmentRegistration> AssignmentRegistrations { get; set; } = new List<AssignmentRegistration>();

    public virtual ICollection<AssignmentScore> AssignmentScores { get; set; } = new List<AssignmentScore>();

    public virtual Subject Subject { get; set; } = null!;
}
