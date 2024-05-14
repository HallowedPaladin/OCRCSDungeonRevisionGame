using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace InsigniaServer.Entities;

public partial class AnswerBlob
{
    public int AnswerBlobId { get; set; }

    public byte[] AnswerBlob1 { get; set; } = null!;

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<AnswersAnswerBlob> AnswersAnswerBlobs { get; set; } = new List<AnswersAnswerBlob>();
}
