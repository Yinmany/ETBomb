namespace AkaUI
{
    public static class AkaBuilderEventBusExtensions
    {
        public static AkaBuilder UseEventBus(this AkaBuilder self)
        {
            return self.CustomAttributeFilter<EventReceiverAttribute>((type, attr) => { EventBus.Subscribe(type); });
        }
    }
}