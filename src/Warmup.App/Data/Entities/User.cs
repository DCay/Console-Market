namespace Warmup.App.Data.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string RoleId { get; set; }

        public UserRole Role { get; set; }
    }
}
