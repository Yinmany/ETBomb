using System.ComponentModel;

namespace Bomb
{
    /// <summary>
    /// 牌的花色
    /// </summary>
    public enum CardColor: byte
    {
        [Description("")]
        None,

        [Description("梅花")]
        Club,

        [Description("方块")]
        Diamond,

        [Description("红桃")]
        Heart,

        [Description("黑桃")]
        Spade
    }

    /// <summary>
    /// 牌的权重
    /// </summary>
    public enum CardWeight: byte
    {
        [Description("3")]
        _3,

        [Description("4")]
        _4,

        [Description("5")]
        _5,

        [Description("6")]
        _6,

        [Description("7")]
        _7,

        [Description("8")]
        _8,

        [Description("9")]
        _9,

        [Description("10")]
        _10,

        [Description("J")]
        J,

        [Description("Q")]
        Q,

        [Description("K")]
        K,

        [Description("A")]
        A,

        [Description("2")]
        _2,

        [Description("小王")]
        SJoker,

        [Description("大王")]
        LJoker
    }

    /// <summary>
    /// 出的牌类型
    /// 玩法：罚王不大
    /// </summary>
    public enum CardType: byte
    {
        None,               // 错误牌型
        Single,             // 单张
        Double,             // 对子
        OnlyThree,          // 三张：牌点相同的三张牌（只有在手牌＜5张时才可以单出，别人也只能用三张或炸弹接牌）
        DoubleStraight,     // 连对：牌点连续的两个（含两个）以上的对子叫对顺。（2不能在连对中）
        ThreeAndTwo,        // 三带二：牌点相同的三张牌+随便两张牌（两张牌可以为单牌）
        TripleStraight,     // 连三带二：牌点连续的两个（含两个）以上的三张+随意4张（6张、8张、、、，2不出现连三中）
        Straight,           // 顺子： 五张或更多的连续单牌
        Boom,               // 炸弹：四个（含四个）以上相同点的牌叫炸弹，可以炸任何类型的牌，炸弹的大小 按数量及2＞A＞K＞Q＞J＞10＞9……..3排序，相同数目的炸按先出顺序比大小（如：四个3先出，则后出的四个3不能压）。
        JokerBoom,          // 王炸
        // FiveTenKing,        // 五十K
    }
}