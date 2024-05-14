using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class QuestionPack
{
    public int QuestionPackId { get; set; }

    public string QuestionPackName { get; set; } = null!;

    public string? QuestionPackDescription { get; set; }

    public sbyte IsPublished { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<AssignmentQuestionPack> AssignmentQuestionPacks { get; set; } = new List<AssignmentQuestionPack>();

    public virtual ICollection<QuestionPackQuestion> QuestionPackQuestions { get; set; } = new List<QuestionPackQuestion>();

    public virtual ICollection<QuestionPackRegistration> QuestionPackRegistrations { get; set; } = new List<QuestionPackRegistration>();

    public virtual ICollection<QuestionPackResponse> QuestionPackResponses { get; set; } = new List<QuestionPackResponse>();

    public virtual ICollection<QuestionPackScore> QuestionPackScores { get; set; } = new List<QuestionPackScore>();
}
