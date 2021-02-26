namespace Bomb.CardPrompt
{
    public interface IAnalyzer
    {
        public bool Check(CardType targetType);

        public void Invoke(AnalysisContext context);
    }
}