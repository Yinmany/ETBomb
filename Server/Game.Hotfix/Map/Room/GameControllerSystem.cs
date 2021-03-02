using System;
using System.Collections.Generic;
using System.Linq;
using ET;

namespace Bomb
{
    public class GameControllerStartSystem: StartSystem<GameController>
    {
        public override void Start(GameController self)
        {
            self.GameStart();
        }
    }

    public static class GameControllerSystem
    {
        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="self"></param>
        public static void GameStart(this GameController self)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = room.GetComponent<GameInfo>();

            // 局数+1
            ++gameInfo.Count;

            // 重置
            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                item.GetComponent<HandCardsComponent>().Cards.Clear();
                item.LastPlayCards = null;
                item.Action = PlayerAction.None;
                item.IsReady = false;
            }

            // 先锁定房间
            room.Lock();

            // 初始化两幅牌
            gameInfo.Cards.Clear();
            CardsHelper.Spawn(gameInfo.Cards);
            CardsHelper.Spawn(gameInfo.Cards);

            // 洗牌
            CardsHelper.Shuffle1(gameInfo.Cards, 3);

            // 发牌
            self.Deal();

            // 初始化队伍以及确定先手玩家
            self.InitFristPlayer();
            self.InitTeam();

            // 同步队伍、手牌
            self.SyncTeam();
            self.SyncHandCard();
            self.SyncGameStart();

            // 摊牌了，不用开始游戏直接结束这局。
            if (self.CheckTanCard())
            {
                self.OnRoundEnd(true);
            }
            else
            {
                self.Continue(false);
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <param name="self"></param>
        private static void Deal(this GameController self)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = self.Parent.GetComponent<GameInfo>();

            // 给测试发牌玩家发牌
            foreach (Player player in room.Players)
            {
                var test = player.GetComponent<TestCardComponent>();
                if (test != null)
                {
                    // 先从牌库中把测试Card提取出来
                    HandCardsComponent handCardsComponent = player.GetComponent<HandCardsComponent>();
                    handCardsComponent.Cards.Clear();

                    foreach (CardProto cardProto in test.Cards)
                    {
                        int idx = gameInfo.Cards.FindIndex(f => f.Weight == (CardWeight) cardProto.Weight);
                        handCardsComponent.Cards.Add(gameInfo.Cards[idx]);
                        gameInfo.Cards.RemoveAt(idx);
                    }

                    handCardsComponent.Cards.AddRange(gameInfo.Cards.GetRange(0, 27 - test.Cards.Count));
                    break;
                }
            }

            // 发牌每人27张
            int index = 0;
            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                // 跳过测试发牌的玩家
                if (item.GetComponent<TestCardComponent>() != null)
                {
                    item.RemoveComponent<TestCardComponent>();
                    continue;
                }

                HandCardsComponent handCardsComponent = item.GetComponent<HandCardsComponent>();
                handCardsComponent.Cards.AddRange(gameInfo.Cards.GetRange(index * 27, 27));
                handCardsComponent.Sort();
                index++;
            }
        }

        /// <summary>
        /// 判定队伍，双扣后重新频断队伍。
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void InitTeam(this GameController self)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = self.Parent.GetComponent<GameInfo>();

            // 首轮默认双扣，需要找队友.
            if (!gameInfo.IsDoubleEnd)
            {
                return;
            }

            // 重置队伍
            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                item.GetComponent<TeamComponent>().Team = TeamType.A;
            }

            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                HandCardsComponent handCardsComponent = item.GetComponent<HandCardsComponent>();
                int count = handCardsComponent.Cards.Count(f => f.Color == CardColor.Spade && f.Weight == CardWeight._3);

                if (count > 0)
                {
                    item.GetComponent<TeamComponent>().Team = TeamType.B;
                }

                // 同个人拿到双黑桃3
                if (count != 2)
                {
                    continue;
                }

                // 直接找到另一半
                int teamBSeatIndex = (i + 2) % 4;
                room.Players[teamBSeatIndex].GetComponent<TeamComponent>().Team = TeamType.B;
                break;
            }
        }

        /// <summary>
        /// 设置先手玩家
        /// </summary>
        /// <param name="self"></param>
        public static void InitFristPlayer(this GameController self)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = self.Parent.GetComponent<GameInfo>();
            // 首轮谁先抓到黑桃3为先出牌
            if (gameInfo.Count == 1)
            {
                for (int i = 0; i < room.Players.Length; i++)
                {
                    var item = room.Players[i];
                    HandCardsComponent handCardsComponent = item.GetComponent<HandCardsComponent>();
                    if (!handCardsComponent.Cards.Any(f => f.Color == CardColor.Spade && f.Weight == CardWeight._3))
                    {
                        continue;
                    }

                    // 设置先手玩家
                    self.LastOpSeat = self.DeskSeat = self.CurrentSeat = i;
                    break;
                }
            }

            // 后面谁为上游先出牌
            if (gameInfo.Count > 1)
            {
                // 设置先手玩家
                self.LastOpSeat = self.DeskSeat = self.CurrentSeat = gameInfo.PrevWinSeat;
            }

            Log.Debug($"[Room:{self.GetParent<Room>().Num}] 设置先手:{self.CurrentSeat}");
        }

        /// <summary>
        /// 同步游戏
        /// </summary>
        /// <param name="self"></param>
        public static void SyncTurn(this GameController self)
        {
            Room room = self.GetParent<Room>();

            // 轮转消息
            TurnMessage msg = new TurnMessage();
            msg.CurrentSeat = self.CurrentSeat;
            msg.LastOpSeat = self.LastOpSeat;

            // 最后操作的玩家
            Player lastPlayer = room.Players[self.LastOpSeat];
            msg.LastOp = (int) lastPlayer.Action;

            // 最后操作是出了牌的，才同步牌桌上的牌。
            if (lastPlayer.Action == PlayerAction.Play)
            {
                msg.DeskSeat = self.DeskSeat;
                msg.DeskCards = self.DeskCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }).ToList();
                msg.DeskCardType = (int) self.DeskCardType;
            }

            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                room.SendActor(item, msg);
            }

            Log.Debug($"[Room:{self.GetParent<Room>().Num}] 轮转消息:{self.CurrentSeat}");
        }

        /// <summary>
        /// 同步手牌
        /// </summary>
        /// <param name="self"></param>
        private static void SyncGameStart(this GameController self)
        {
            Room room = self.GetParent<Room>();
            GameStartMessage msg = new GameStartMessage();
            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                room.SendActor(item, msg);
            }
        }

        /// <summary>
        /// 同步手牌
        /// </summary>
        /// <param name="self"></param>
        private static void SyncHandCard(this GameController self)
        {
            Room room = self.GetParent<Room>();
            HandCardMessage msg = new HandCardMessage();
            for (int i = 0; i < room.Players.Length; i++)
            {
                msg.Cards.Clear();
                var item = room.Players[i];
                msg.Seat = item.SeatIndex;
                var handCards = item.GetComponent<HandCardsComponent>().Cards;
                msg.Cards.AddRange(handCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }));
                room.SendActor(item, msg);
            }
        }

        /// <summary>
        /// 同步手牌(广播)
        /// </summary>
        /// <param name="self"></param>
        private static void SyncHandCardAll(this GameController self)
        {
            Room room = self.GetParent<Room>();
            HandCardMessage msg = new HandCardMessage();
            for (int i = 0; i < room.Players.Length; i++)
            {
                msg.Cards.Clear();
                var item = room.Players[i];
                msg.Seat = item.SeatIndex;
                var handCards = item.GetComponent<HandCardsComponent>().Cards;
                msg.Cards.AddRange(handCards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }));

                foreach (Player player in room.Players)
                {
                    room.SendActor(player, msg);
                }
            }
        }

        /// <summary>
        /// 同步队伍
        /// </summary>
        /// <param name="self"></param>
        private static void SyncTeam(this GameController self)
        {
            Room room = self.GetParent<Room>();
            TeamMessage teamMsg = new TeamMessage();
            for (int i = 0; i < room.Players.Length; i++)
            {
                var item = room.Players[i];
                teamMsg.Team = (int) item.GetComponent<TeamComponent>().Team;
                teamMsg.SeatIndex = item.SeatIndex;

                // 把team发给每个人
                for (int j = 0; j < room.Players.Length; j++)
                {
                    room.SendActor(room.Players[j], teamMsg);
                }
            }
        }

        /// <summary>
        /// 同步分数
        /// </summary>
        /// <param name="self"></param>
        private static void SyncScore(this GameController self)
        {
            Room room = self.GetParent<Room>();
            for (int i = 0; i < room.Players.Length; i++)
            {
                var p = room.Players[i];
                var msg = new ScoreMessage { Seat = p.SeatIndex, Score = p.GetComponent<ScoreComponent>().Score };

                foreach (Player player in room.Players)
                {
                    room.SendActor(player, msg);
                }
            }
        }

        /// <summary>
        /// 一个玩家出牌后调用
        /// </summary>
        /// <param name="self"></param>
        /// <param name="turn"></param>
        public static void Continue(this GameController self, bool turn = true)
        {
            if (turn)
            {
                // 轮转到下一个人
                self.CurrentSeat = (self.CurrentSeat + 1) % 4;
            }

            // 接风
            self.Windup();

            self.SyncTurn();
        }

        private static void Windup(this GameController self)
        {
            // 只有当有一个玩家已经出完牌，才会进行接风判定。
            if (self.Win.Count != 1)
            {
                return;
            }

            int winSeat = self.Win.Peek();
            if (winSeat != self.CurrentSeat)
            {
                return;
            }

            // 再次轮到出完牌的人出牌
            if (self.DeskSeat == winSeat && !self.IsWindup)
            {
                var room = self.GetParent<Room>();

                // 出完牌的人
                var player = room.Players[self.CurrentSeat];

                // 接风
                Player friend = room.GetSameTeam(player);
                self.CurrentSeat = friend.SeatIndex;
                self.DeskCards.Clear();
                self.DeskSeat = self.CurrentSeat;
                self.DeskCardType = CardType.None;

                self.IsWindup = true;
            }
            else
            {
                // 跳过此玩家
                self.CurrentSeat = (self.CurrentSeat + 1) % 4;
            }
        }

        /// <summary>
        /// 不出
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool NotPop(this GameController self, Player player)
        {
            // 不该此玩家操作
            if (self.CurrentSeat != player.SeatIndex)
            {
                return false;
            }

            if (self.DeskSeat == self.CurrentSeat)
            {
                return false;
            }

            // 记录Player最后一次的操作，断线重连后需要恢复每个玩家的最近的操作。
            self.LastOpSeat = player.SeatIndex;
            player.Action = PlayerAction.NotPlay;
            player.LastPlayCards?.Clear();

            self.Continue();
            return true;
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="cards"></param>
        public static bool Pop(this GameController self, Player player, List<Card> cards)
        {
            // 不该此玩家操作
            if (self.CurrentSeat != player.SeatIndex)
            {
                return false;
            }

            // 没有牌
            if (cards == null || cards.Count == 0)
            {
                return false;
            }

            // 验证出牌是否符合
            if (!cards.TryGetCardType(out CardType type))
            {
                // 出牌失败，牌型不对.
                return false;
            }

            // 特殊排序
            if (type == CardType.ThreeAndTwo || type == CardType.TripleStraight)
            {
                // 权重排序
                CardsHelper.WeightSort(cards);
            }

            // 接牌，自己上次出牌没人接牌以及，自己能接上次别人出的牌，就轮转。
            // self.LastPopCardSeat == self.CurrentPopSeat 是自己的牌最大，都没有人要。
            // 一般来说都得同牌型，但炸弹能接所有牌型。
            if (self.DeskSeat != self.CurrentSeat && !PopCardHelper.Pop(new PopCheckInfo
            {
                DesktopCards = self.DeskCards,
                DesktopCardType = self.DeskCardType,
                HandCards = player.GetComponent<HandCardsComponent>().Cards.Count,
                PopCards = cards,
                PopCardType = type
            }))
            {
                return false;
            }

            // 接牌成功
            self.DeskCards = cards;
            self.DeskCardType = type;
            self.DeskSeat = player.SeatIndex;
            self.LastOpSeat = player.SeatIndex;

            // 从玩家手牌中移除牌
            var handCards = player.GetComponent<HandCardsComponent>();
            handCards.Remove(cards);

            // 记录Player最后一次的操作，断线重连后需要恢复每个玩家的最近的操作。
            player.Action = PlayerAction.Play;
            player.LastPlayCards = cards.ToList();

            // 下一个人出牌
            self.Continue();

            // 玩家出完牌
            if (handCards.Cards.Count == 0)
            {
                self.Win.Enqueue(player.SeatIndex);
                if (self.Win.Count == 2) // 一局结束
                {
                    Room room = self.GetParent<Room>();
                    var gameInfo = room.GetComponent<GameInfo>();

                    // 判定是否双扣
                    int a = self.Win.Dequeue();
                    gameInfo.IsDoubleEnd = room.Players[a].GetComponent<TeamComponent>().Team == player.GetComponent<TeamComponent>().Team;
                    OnRoundEnd(self);
                }
            }

            return true;
        }

        /// <summary>
        /// 一局游戏结束
        /// </summary>
        /// <param name="self"></param>
        /// <param name="startTan">是否一发后牌就进行摊牌的</param>
        private static void OnRoundEnd(this GameController self, bool startTan = false)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = room.GetComponent<GameInfo>();

            // 一开始就摊牌 需要罚王以及炸弹
            if (startTan)
            {
                self.FetchJokerBoomScore();
                self.JokerScore();
                Log.Debug($"一局结束:摊牌...");
            }
            else // 一局结束的摊牌
            {
                // 双扣需罚王
                if (gameInfo.IsDoubleEnd)
                {
                    self.JokerScore();
                    Log.Debug($"一局结束:双扣...");
                }
                else
                {
                    self.FetchJokerBoomScore();
                    Log.Debug($"一局结束:单扣...");
                }
            }

            self.SyncScore();
            self.SyncHandCardAll();
            foreach (Player player in room.Players)
            {
                room.SendActor(player, new RoundEndMessage { });
            }

            // 移除游戏控制器，游戏需要开始时，添加控制器即可.控制器只保留一局的状态
            room.RemoveComponent<GameController>();

            Log.Debug($"结束移除控制器:{room.GetComponent<GameController>() == null}");
        }

        /// <summary>
        /// 摊牌检测
        /// 1. 有王没炸弹
        /// 2. 达到炸弹封顶
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private static bool CheckTanCard(this GameController self)
        {
            Room room = self.GetParent<Room>();
            var gameInfo = room.GetComponent<GameInfo>();
            foreach (Player player in room.Players)
            {
                var cards = player.GetComponent<HandCardsComponent>().Cards;
                List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);
                int jokerCount = analyseResults.GetJokerCount();
                if (jokerCount == 4)
                {
                    return true;
                }

                // 炸弹
                analyseResults = analyseResults.GetBooms();

                // 有王没有炸弹
                if (jokerCount > 0 && analyseResults.Count == 0)
                {
                    return true;
                }

                // 炸弹封顶
                if (analyseResults.Any(result => result.Count + jokerCount >= gameInfo.BoomTop))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 配王算分
        /// </summary>
        /// <param name="self"></param>
        private static void FetchJokerBoomScore(this GameController self)
        {
            var room = self.GetParent<Room>();
            var gameInfo = self.Parent.GetComponent<GameInfo>();

            foreach (Player player in room.Players)
            {
                List<Card> cards = player.GetComponent<HandCardsComponent>().Cards;
                if (cards.Count == 0)
                {
                    continue;
                }

                List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);
                int jokerCount = analyseResults.GetJokerCount();
                if (jokerCount == 4)
                {
                    self.BoomScore(player, gameInfo.BoomTop);
                    jokerCount = 0;
                }

                analyseResults = analyseResults.GetBooms();

                // 按照数量降序
                analyseResults.Sort();

                // 配王
                foreach (AnalyseResult analyseResult in analyseResults)
                {
                    // 最多8张牌
                    int boomCount = analyseResult.Count;

                    if (jokerCount > 0)
                    {
                        // 需要多少张王
                        int a = gameInfo.BoomTop - boomCount;

                        // 取小(jokerCount,a)
                        a = a > jokerCount? jokerCount : a;

                        // 减去用了的
                        jokerCount -= a;

                        // 配上的
                        boomCount += a;
                    }

                    self.BoomScore(player, boomCount);
                }
            }
        }

        /// <summary>
        /// 炸弹得分
        /// 低分3，分数是低分翻倍。 5炸3分，6炸6分，7炸12分，8炸24分，9炸48分.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        /// <param name="boomCount"></param>
        private static void BoomScore(this GameController self, Player player, int boomCount)
        {
            if (boomCount < 4)
            {
                return;
            }

            var gameInfo = self.Parent.GetComponent<GameInfo>();

            // 低分
            int score = 3;

            // 5 - 5 = 0
            // 6 - 5 = 1
            // 9 - 5 = 4
            // 3 * 2 * 2 * 2 * 2 = 48
            // 
            if (boomCount > gameInfo.BoomTop)
            {
                boomCount = gameInfo.BoomTop;
            }

            if (boomCount > 5)
            {
                score *= (int) Math.Pow(2, boomCount - 5);
            }

            // 得分
            self.SetScore(player, score);
        }

        /// <summary>
        /// 王罚分
        /// </summary>
        /// <param name="self"></param>
        private static void JokerScore(this GameController self)
        {
            var room = self.GetParent<Room>();
            foreach (Player player in room.Players)
            {
                var cards = player.GetComponent<HandCardsComponent>().Cards;
                if (cards.Count == 0)
                {
                    continue;
                }

                List<AnalyseResult> analyseResults = AnalyseResult.Analyse(cards);

                // 一旦存在炸弹，就不需要罚王了。
                if (analyseResults.Any(f => f.Count >= 4 && !CardsHelper.IsJoker(f.Weight)))
                {
                    continue;
                }

                int jokerCount = analyseResults.GetJokerCount();

                // joker数量为4不需要罚王，算做9炸分.
                if (jokerCount == 4)
                {
                    continue;
                }

                // 罚分
                self.SetScore(player, -(jokerCount * 3));
            }
        }

        private static void SetScore(this GameController self, Player player, int score)
        {
            Room room = self.GetParent<Room>();
            foreach (Player item in room.Players)
            {
                ScoreComponent scoreComponent = item.GetComponent<ScoreComponent>();
                if (item == player)
                {
                    scoreComponent.Score += score;
                }
                else
                {
                    scoreComponent.Score -= score / 3;
                }
            }
        }
    }
}