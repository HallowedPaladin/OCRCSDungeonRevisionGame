using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class QuestionsQuestionBlob
{
    public int QuestionId { get; set; }

    public int QuestionBlobId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual QuestionBlob QuestionBlob { get; set; } = null!;
}
