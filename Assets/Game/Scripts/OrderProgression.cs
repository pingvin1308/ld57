using System;

namespace Game.Scripts
{
    [Serializable]
    public class OrderProgression
    {
        public int CompletedOrders { get; set; }

        public OrderData GetNext()
        {
            var minScale = CompletedOrders + 1;
            return new OrderData(10 * minScale, 10 * minScale);
        }
    }
}