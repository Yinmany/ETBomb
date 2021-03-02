using System.Collections.Generic;
using System.Linq;

namespace Bomb
{
    public static class AnalyseResultHelper
    {
        /// <summary>
        /// 获取Joker的数量
        /// </summary>
        /// <param name="analyseResults"></param>
        /// <returns></returns>
        public static int GetJokerCount(this List<AnalyseResult> analyseResults)
        {
            int jokerCount = analyseResults.Sum(f =>
            {
                if (CardsHelper.IsJoker(f.Weight))
                {
                    return f.Count;
                }

                return 0;
            });
            return jokerCount;
        }

        /// <summary>
        /// 筛选炸弹
        /// </summary>
        /// <param name="analyseResults"></param>
        /// <returns></returns>
        public static List<AnalyseResult> GetBooms(this List<AnalyseResult> analyseResults)
        {
            return analyseResults.Where(f => f.Count > 3 && !CardsHelper.IsJoker(f.Weight)).ToList();
        }
    }
}