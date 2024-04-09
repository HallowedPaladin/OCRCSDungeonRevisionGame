using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public int QuestionTypeId { get; set; }

    public int QuestionValue { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual QuestionType QuestionType { get; set; } = null!;

    public virtual ICollection<QuestionBlob> QuestionBlobs { get; set; } = new List<QuestionBlob>();
}
