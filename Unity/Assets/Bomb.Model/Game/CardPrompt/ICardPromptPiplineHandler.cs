namespace Bomb
{
    public interface ICardPromptPiplineHandler
    {
        public bool Check(CardsType targetType);

        public void Invoke(CardPromptPiplineContext context);
    }
}