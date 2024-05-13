using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionPackRegistration
{
    public int QuestionPackRegistrationId { get; set; }

    public int AssignmentRegistrationId { get; set; }

    public int QuestionPackId { get; set; }

    public sbyte IsSubmitted { get; set; }

    public DateTime RegistrationDateTime { get; set; }

    public DateTime? SubmissionDateTime { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual AssignmentRegistration AssignmentRegistration { get; set; } = null!;

    public virtual QuestionPack QuestionPack { get; set; } = null!;
}
