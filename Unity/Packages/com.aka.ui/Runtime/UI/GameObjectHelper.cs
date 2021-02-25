using System;
using UnityEngine;

namespace AkaUI
{
    public static class GameObjectHelper
    {
        public static T Get<T>(this GameObject gameObject, string key) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return null;

                T t = gameObject.GetComponent<ReferenceCollector>().Get<T>(key);

                return t;
            }
            catch (Exception)
            {
                if (key != "BtnGoBack")
                {
                    Debug.LogError($"获取{gameObject.name} 的 {typeof(T).Name} 失败, key: {key}");
                }
            }

            return null;
        }

        public static T GetEx<T>(this GameObject gameObject, string key) where T : class
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector>().Get<GameObject>(key).GetComponent<T>();
            }
            catch (Exception)
            {
                if (key != "BtnGoBack")
                {
                    Debug.LogError($"获取{gameObject.name} 的 {typeof(T).Name} 失败, key: {key}");
                }
            }

            return null;
        }


        /// <summary>
        /// 直接获取GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static GameObject Get(this GameObject gameObject, string key)
        {
            try
            {
                return gameObject.Get<GameObject>(key);
            }
            catch (Exception)
            {
                if (key != "BtnGoBack")
                {
                    Debug.LogError($"获取{gameObject.name} 失败, key: {key}");
                }
            }

            return null;
        }
    }
}