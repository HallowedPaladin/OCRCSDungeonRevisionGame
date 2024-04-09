using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionBlob
{
    public int QuestionBlobId { get; set; }

    public byte[] QuestionBlob1 { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
