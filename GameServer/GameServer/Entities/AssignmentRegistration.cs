using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class AssignmentRegistration
{
    public int AssignmentRegistrationId { get; set; }

    public int UserId { get; set; }

    public int AssignmentId { get; set; }

    public sbyte IsSubmitted { get; set; }

    public DateTime RegistrationDateTime { get; set; }

    public DateTime? SubmissionDateTime { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual ICollection<QuestionPackRegistration> QuestionPackRegistrations { get; set; } = new List<QuestionPackRegistration>();

    public virtual User User { get; set; } = null!;
}
