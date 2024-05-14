using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class AssignmentQuestionPack
{
    public int AssignmentId { get; set; }

    public int QuestionPackId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual QuestionPack QuestionPack { get; set; } = null!;
}
