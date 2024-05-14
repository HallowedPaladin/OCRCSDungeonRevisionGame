using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class Answer
{
    public int AnswerId { get; set; }

    public string AnswerText { get; set; } = null!;

    public int AnswerValue { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<AnswersAnswerBlob> AnswersAnswerBlobs { get; set; } = new List<AnswersAnswerBlob>();

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<QuestionPackResponse> QuestionPackResponses { get; set; } = new List<QuestionPackResponse>();
}
