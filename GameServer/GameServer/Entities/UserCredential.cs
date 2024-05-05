using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameServer.Entities;

public partial class UserCredential
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    [Timestamp]
	public DateTime Timestamp { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
