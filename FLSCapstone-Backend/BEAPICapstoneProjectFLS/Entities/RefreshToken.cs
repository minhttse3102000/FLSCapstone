using System;

namespace BEAPICapstoneProjectFLS.Entities
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public int IsUsed { get; set; }
        public int IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public virtual User User { get; set; }
    }
}
