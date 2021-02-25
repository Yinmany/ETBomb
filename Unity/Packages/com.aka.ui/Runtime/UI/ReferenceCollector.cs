using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace AkaUI
{
    [Serializable]
    public class ReferenceCollectorData
    {
        public string key;

        public Object gameObject;

        public GameObject GameObject => (GameObject) gameObject;

        public bool ignore;
    }

    public class ReferenceCollectorDataComparer: IComparer<ReferenceCollectorData>
    {
        public int Compare(ReferenceCollectorData x, ReferenceCollectorData y)
        {
            return string.Compare(x.key, y.key, StringComparison.Ordinal);
        }
    }

    public class ReferenceCollector: MonoBehaviour, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        public string scriptGuid = null;
#endif

        public List<ReferenceCollectorData> data = new List<ReferenceCollectorData>();

        private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();

        public T Get<T>(string key) where T : class
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo))
            {
                return null;
            }

            return dictGo as T;
        }

        public Object Get(string key)
        {
            Object dictGo;
            if (!dict.TryGetValue(key, out dictGo))
            {
                return null;
            }

            return dictGo;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            dict.Clear();
            foreach (ReferenceCollectorData referenceCollectorData in data)
            {
                if (!dict.ContainsKey(referenceCollectorData.key))
                {
                    dict.Add(referenceCollectorData.key, referenceCollectorData.gameObject);
                }
            }
        }
#if UNITY_EDITOR

        private void Reset()
        {
            this.FindUIScript();
        }

        public void FindUIScript()
        {
            string filter = $"{this.gameObject.name} t:Script";
            string[] guids = AssetDatabase.FindAssets(filter);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.EndsWith(".Gen.cs"))
                {
                    this.scriptGuid = guids[0];
                    Debug.Log($"脚本:[{filter}]{path} {guid}");
                    break;
                }
            }
        }
#endif
    }
}