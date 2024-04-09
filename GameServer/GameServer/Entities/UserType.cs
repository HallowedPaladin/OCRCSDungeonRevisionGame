using System;
using System.Collections.Generic;

namespace GameServer.Entities;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string UserTypeName { get; set; } = null!;

    public string UserTypeDesc { get; set; } = null!;

    public DateTime UpdateDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
