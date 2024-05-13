using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionTitle { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public int QuestionTypeId { get; set; }

    public int QuestionValue { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<QuestionPackQuestion> QuestionPackQuestions { get; set; } = new List<QuestionPackQuestion>();

    public virtual ICollection<QuestionPackResponse> QuestionPackResponses { get; set; } = new List<QuestionPackResponse>();

    public virtual QuestionType QuestionType { get; set; } = null!;

    public virtual ICollection<QuestionsQuestionBlob> QuestionsQuestionBlobs { get; set; } = new List<QuestionsQuestionBlob>();
}
