using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class ClassroomSubject
{
    public int ClassroomId { get; set; }

    public int SubjectId { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
