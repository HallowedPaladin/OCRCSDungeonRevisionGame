using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public string ClassroomName { get; set; } = null!;

    public string? ClassroomDescription { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
