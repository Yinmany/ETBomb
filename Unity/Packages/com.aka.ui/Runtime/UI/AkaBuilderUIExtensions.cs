namespace AkaUI
{
    public static class AkaBuilderUIExtensions
    {
        public static AkaBuilder UseUI(this AkaBuilder self)
        {
            return self.CustomAttributeFilter<UIAttribute>(Akau.AddProvider);
        }
    }
}