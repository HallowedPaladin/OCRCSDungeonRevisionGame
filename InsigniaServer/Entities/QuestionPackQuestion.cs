using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionPackQuestion
{
    public int QuestionPackId { get; set; }

    public int QuestionId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual QuestionPack QuestionPack { get; set; } = null!;
}
