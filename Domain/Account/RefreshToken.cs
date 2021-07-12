using System;

namespace Domain.Account
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Disabled { get; set; }
        public bool IsActive => Disabled == null && !IsExpired;
    }
}