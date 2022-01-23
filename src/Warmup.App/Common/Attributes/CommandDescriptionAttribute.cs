namespace Warmup.App.Common.Attributes
{
    public class CommandDescriptionAttribute : Attribute
    {
        public CommandDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}
