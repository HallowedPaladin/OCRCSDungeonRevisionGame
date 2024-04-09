using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Answer
{
    public int AnswerId { get; set; }

    public string AnswerText { get; set; } = null!;

    public int AnswerValue { get; set; }

    public DateTime UpdateTime { get; set; }

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<AnswerBlob> AnswerBlobs { get; set; } = new List<AnswerBlob>();
}
