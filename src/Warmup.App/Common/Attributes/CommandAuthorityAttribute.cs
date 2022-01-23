namespace Warmup.App.Common.Attributes
{
    public class CommandAuthorityAttribute : Attribute
    {
        public CommandAuthorityAttribute(params string[] authorities)
        {
            this.Authorities = new List<string>(authorities);
        }

        public List<string> Authorities { get; private set; }
    }
}
