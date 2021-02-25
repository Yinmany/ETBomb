using System.Collections.Generic;
using Bomb.Handler;
using ET;

namespace Bomb
{
    public class HandCardsComponentAwakeSystem : AwakeSystem<HandCardsComponent>
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
        public List<Card> Cards;

        private CardPromptPipline pipline = new CardPromptPipline();

        private bool _isReprompt = true;

        public int PromptIndex { get; set; }

        public void Awake()
        {
            pipline.Handlers.AddLast(new SingleHandler());
            pipline.Handlers.AddLast(new DoubleHandler());
            pipline.Handlers.AddLast(new OnlyThreeHandler());
            pipline.Handlers.AddLast(new DoubleStraightHandler());
            pipline.Handlers.AddLast(new ThreeAndTwoHandler());
            pipline.Handlers.AddLast(new TripleStraightHandler());
            pipline.Handlers.AddLast(new StraightHandler());
            pipline.Handlers.AddLast(new BomHandler());
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
                var game = player.Room.GetComponent<GameControllerComponent>();
                pipline.Run(this.Cards, game.DeskCards, game.DeskCardsType);
                this._isReprompt = false;
            }

            if (this.pipline.PrompCardsList.Count <= 0)
            {
                return null;
            }

            this.PromptIndex = (this.PromptIndex + 1) % this.pipline.PrompCardsList.Count;
            return this.pipline.PrompCardsList[this.PromptIndex].Cards;
        }
    }
}