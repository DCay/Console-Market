namespace Warmup.App.Core.Models.Core
{
    public class Authentication
    {
        public string User { get; set; }

        public string Role { get; set; }

        public bool IsAuthenticated { get; set; }

        public Dictionary<string, object> SessionData { get; set; } = new Dictionary<string, object>();
    }
}
