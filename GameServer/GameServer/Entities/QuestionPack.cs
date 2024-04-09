using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionPack
{
    public int QuestionPackId { get; set; }

    public string QuestionPackName { get; set; } = null!;

    public string? QuestionPackDescription { get; set; }

    public DateTime UpdateDate { get; set; }
}
