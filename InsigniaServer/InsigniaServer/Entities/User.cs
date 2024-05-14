using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InsigniaServer.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public string UserName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? FamilyName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public sbyte? IsEmailVerified { get; set; }

    public int? PhoneCountryCode { get; set; }

    public string? PhoneNumber { get; set; }

    public sbyte? IsPhoneVerified { get; set; }

    public DateTime RegistrationDate { get; set; }

    [Timestamp]
	public DateTime Timestamp { get; set; }

    [JsonIgnore]
    public virtual ICollection<AssignmentRegistration> AssignmentRegistrations { get; set; } = new List<AssignmentRegistration>();

    [JsonIgnore]
    public virtual ICollection<AssignmentScore> AssignmentScores { get; set; } = new List<AssignmentScore>();

    [JsonIgnore]
    public virtual ICollection<AuditRecord> AuditRecords { get; set; } = new List<AuditRecord>();

    [JsonIgnore]
    public virtual Preference? Preference { get; set; }

    [JsonIgnore]
    public virtual ICollection<QuestionPackScore> QuestionPackScores { get; set; } = new List<QuestionPackScore>();

    [JsonIgnore]
    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();

    [JsonIgnore]
    public virtual UserCredential? UserCredential { get; set; }

    [JsonIgnore]
    public virtual UserLogon? UserLogon { get; set; }

    [JsonIgnore]
    public virtual UserType UserType { get; set; } = null!;
}
