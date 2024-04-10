using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class UserType
{
    [Key]
    public int UserTypeId { get; set; }

    public string UserTypeName { get; set; } = null!;

    public string UserTypeDesc { get; set; } = null!;

    [Timestamp]
	public DateTime Timestamp { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
