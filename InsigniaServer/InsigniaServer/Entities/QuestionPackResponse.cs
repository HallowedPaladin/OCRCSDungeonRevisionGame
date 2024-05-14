using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class QuestionPackResponse
{
    public string QuestionPackRegistrationId { get; set; } = null!;

    public int QuestionPackId { get; set; }

    public int QuestionId { get; set; }

    public int AnswerId { get; set; }

    public DateTime ResponseDateTime { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;

    public virtual QuestionPack QuestionPack { get; set; } = null!;
}
