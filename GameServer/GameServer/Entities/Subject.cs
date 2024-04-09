using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public string? SubjectDescription { get; set; }

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
