using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public string UserName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? FamilyName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<AssignmentScore> AssignmentScores { get; set; } = new List<AssignmentScore>();

    public virtual ICollection<AuditRecord> AuditRecords { get; set; } = new List<AuditRecord>();

    public virtual Password? Password { get; set; }

    public virtual Preference? Preference { get; set; }

    public virtual UserLogon? UserLogon { get; set; }

    public virtual UserType UserType { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
