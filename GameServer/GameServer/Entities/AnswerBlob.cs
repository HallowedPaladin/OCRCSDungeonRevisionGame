using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class AnswerBlob
{
    public int AnswerBlobId { get; set; }

    public byte[] AnswerBlob1 { get; set; } = null!;

    public DateTime UpdateTime { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
