using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class AnswersAnswerBlob
{
    public int AnswerId { get; set; }

    public int AnswerBlobId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual AnswerBlob AnswerBlob { get; set; } = null!;
}
