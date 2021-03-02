using System.Collections.Generic;
using Bomb.CardPrompt;
using ET;

namespace Bomb
{
    public class HandCardsComponentAwakeSystem: AwakeSystem<HandCardsComponent>
    {
        public override void Awake(HandCardsComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 手牌组件
    /// </summary>
    public class HandCardsComponent: Entity
    {
        public List<Card> Cards { get; } = new List<Card>();

        private readonly CardPromptAnalysis analysis = new CardPromptAnalysis();

        private bool _isReprompt = true;

        public int PromptIndex { get; private set; }

        public void Awake()
        {
            this.analysis.AddLast(new SingleAnalyzer());
            this.analysis.AddLast(new DoubleAnalyzer());
            this.analysis.AddLast(new OnlyThreeAnalyzer());
            this.analysis.AddLast(new DoubleStraightAnalyzer());
            this.analysis.AddLast(new ThreeAndTwoAnalyzer());
            this.analysis.AddLast(new TripleStraightAnalyzer());
            this.analysis.AddLast(new StraightAnalyzer());
            this.analysis.AddLast(new BomAnalyzer());
        }

        public void Sort()
        {
            Cards.Sort();
        }
        
        /// <summary>
        /// 出牌成功调用，移除出了牌。
        /// </summary>
        /// <param name="cards"></param>
        public void Remove(IEnumerable<Card> cards)
        {
            Reprompt();

            if (this.Cards == null)
            {
                return;
            }

            foreach (var card in cards)
            {
                Cards.Remove(card);
            }
        }

        /// <summary>
        /// 需要重新提示
        /// </summary>
        public void Reprompt()
        {
            _isReprompt = true;
        }

        public List<Card> GetPrompt()
        {
            if (this._isReprompt)
            {
                PromptIndex = -1;

                // 调用提示
                Player player = this.GetParent<Player>();
                var game = player.Room.GetComponent<GameController>();
                this.analysis.Run(this.Cards, game.DeskCards, game.DeskCardType);
                this._isReprompt = false;
            }

            if (this.analysis.PrompCardsList.Count <= 0)
            {
                return null;
            }

            this.PromptIndex = (this.PromptIndex + 1) % this.analysis.PrompCardsList.Count;
            return this.analysis.PrompCardsList[this.PromptIndex].Cards;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            Cards.Clear();
            analysis.Clear();
            _isReprompt = true;
            this.PromptIndex = -1;
        }
    }
}