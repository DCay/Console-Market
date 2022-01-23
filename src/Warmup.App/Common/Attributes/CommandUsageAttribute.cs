namespace Warmup.App.Common.Attributes
{
    public class CommandUsageAttribute : Attribute
    {
        public CommandUsageAttribute(string usage)
        {
            this.Usage = usage;
        }

        public string Usage { get; private set; }
    }
}
