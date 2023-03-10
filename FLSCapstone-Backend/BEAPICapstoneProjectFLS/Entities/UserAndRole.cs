namespace BEAPICapstoneProjectFLS.Entities
{
    public class UserAndRole
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public int Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
