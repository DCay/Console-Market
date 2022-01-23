namespace Warmup.App.Common.Attributes
{
    public class CommandAliasAttribute : Attribute
    {
        public CommandAliasAttribute(string alias)
        {
            this.Alias = alias;
        }

        public string Alias { get; private set; }
    }
}
