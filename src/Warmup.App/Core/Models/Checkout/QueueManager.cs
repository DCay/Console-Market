namespace Warmup.App.Core.Models.Checkout
{
    public class QueueManager
    {
        public Dictionary<int, Queue<Data.Entities.User>> DeckQueues = new Dictionary<int, Queue<Data.Entities.User>>();
    }
}
