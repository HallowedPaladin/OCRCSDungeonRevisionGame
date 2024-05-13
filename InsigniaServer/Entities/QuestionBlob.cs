using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class QuestionBlob
{
    public int QuestionBlobId { get; set; }

    public byte[] QuestionBlob1 { get; set; } = null!;

    public DateTime Tmestamp { get; set; }

    public virtual ICollection<QuestionsQuestionBlob> QuestionsQuestionBlobs { get; set; } = new List<QuestionsQuestionBlob>();
}
