﻿using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionType
{
    public int QuestionTypeId { get; set; }

    public string QuestionType1 { get; set; } = null!;

    public string? QuestionTypeDescription { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
