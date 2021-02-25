using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bomb;
using ET;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Tests
{
    public static class CardTestHelper
    {
        public static Card _3 = new Card { Color = CardColor.Club, Weight = CardWeight._3 };
        private static Dictionary<string, CardWeight> map = new Dictionary<string, CardWeight>();

        static CardTestHelper()
        {
            Type type = typeof (CardWeight);
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                var attr = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);
                if (attr == null)
                {
                    continue;
                }

                map.Add(attr.Description, (CardWeight) Enum.ToObject(type, fieldInfo.GetValue(type)));
            }
        }

        public static Card Create(string i)
        {
            if (!map.TryGetValue(i, out CardWeight weight))
            {
                Log.Error($"不存在此权重:{i}");
            }

            return new Card { Weight = weight };
        }

        public static void Create(string cards, List<Card> list)
        {
            string[] targetCardsStr = cards.Split(',');
            foreach (string s in targetCardsStr)
            {
                list.Add(Create(s));
            }
        }
    }

    public class PromptTests
    {
        [Test]
        public void TestConfig()
        {
            CardPromptPipline pipline = new CardPromptPipline();

            ConfigHelper.CustomGetText = s =>
            {
                string txt = File.ReadAllText($"Assets/Bundles/Config/{s}.txt");
                return txt;
            };

            PromptTestConfigCategory config = new PromptTestConfigCategory();
            config.BeginInit();
            config.EndInit();

            Dictionary<long, PromptTestConfig> configs = config.GetAll();
            foreach (long key in configs.Keys)
            {
                Log.Debug($"{key}:{MongoHelper.ToJson(configs[key])}");
                Test(configs[key], pipline);
            }
        }

        private void Test(PromptTestConfig config, CardPromptPipline pipline)
        {
            List<Card> target = new List<Card>();
            CardsType targetType = (CardsType) Enum.Parse(typeof (CardsType), config.TargetType);
            List<Card> handCards = new List<Card>();

            CardTestHelper.Create(config.Cards, handCards);
            CardTestHelper.Create(config.Target, target);

            int result = config.Result;

            // 添加提示管道
            pipline.Handlers.Clear();
            var handlersStr = config.Handles.Split(',');
            var assembly = pipline.GetType().Assembly;
            foreach (string s in handlersStr)
            {
                Type type = assembly.GetType($"Bomb.Handler.{s}Handler");
                pipline.Handlers.AddLast((ICardPromptPiplineHandler) Activator.CreateInstance(type));
            }

            pipline.Run(handCards, target, targetType);

            // 提示结果数量不对
            Assert.AreEqual(pipline.PrompCardsList.Count, result);
            
            if (!string.IsNullOrEmpty(config.Prompt))
            {
                string[] groups = config.Prompt.Split('|');

                for (int i = 0; i < pipline.PrompCardsList.Count; i++)
                {
                    var p = pipline.PrompCardsList[i];
                    List<Card> temp = new List<Card>();
                    CardTestHelper.Create(groups[i], temp);

                    // 提示目标里面一组提示不对
                    Assert.AreEqual(p.Cards.Count, temp.Count);

                    for (int j = 0; j < p.Cards.Count; j++)
                    {
                        Assert.AreEqual(p.Cards[j].Weight, temp[j].Weight);
                    }
                }
            }
        }
    }
}