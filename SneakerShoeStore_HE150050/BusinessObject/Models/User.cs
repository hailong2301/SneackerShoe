using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public bool? UserStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
