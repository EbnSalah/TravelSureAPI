namespace TravelSureAPI.Models
{
    public class UserAccount
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string MembershipTier { get; set; }

    }
}
