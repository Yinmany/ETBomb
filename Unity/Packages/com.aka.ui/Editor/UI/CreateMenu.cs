using UnityEditor;
using UnityEngine;

namespace AkaUI.Editor
{
    public static class CreateMenu
    {
        [MenuItem("GameObject/AkaUI/Create", priority = 0)]
        public static void CreateUIGlobal()
        {
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.aka.ui/Editor/AkaUI.prefab");
            var go = PrefabUtility.InstantiatePrefab(gameObject);
            PrefabUtility.UnpackPrefabInstance((GameObject) go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
    }
}