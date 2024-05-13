using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionAnswer
{
    public int QuestionAnswerId { get; set; }

    public int QuestionId { get; set; }

    public int AnswerId { get; set; }

    public bool IsCorrect { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
