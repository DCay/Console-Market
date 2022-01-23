namespace Warmup.App.Core.Base.Views
{
    public interface IView
    {
        Dictionary<string, object> ViewData { get; set; }

        string GetRepresentation();
    }
}
