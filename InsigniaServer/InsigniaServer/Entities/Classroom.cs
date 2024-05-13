using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public string ClassroomName { get; set; } = null!;

    public string? ClassroomDescription { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    public virtual ICollection<ClassroomSubject> ClassroomSubjects { get; set; } = new List<ClassroomSubject>();
}
