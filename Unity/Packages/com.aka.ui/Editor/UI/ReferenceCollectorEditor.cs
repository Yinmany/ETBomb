using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Text;
using AkaUI;
using AkaUI.Editor;
using UnityEditorInternal;
using static UnityEditor.GenericMenu;

[CustomEditor(typeof (ReferenceCollector))]
[CanEditMultipleObjects]
public class ReferenceCollectorEditor: UnityEditor.Editor
{
    private ReferenceCollector referenceCollector;

    private Object heroPrefab;

    private ReorderableList reorderableList;

    private List<int> delList = new List<int>();

    private void DelNullReference()
    {
        var dataProperty = serializedObject.FindProperty("data");
        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
            if (gameObjectProperty.objectReferenceValue == null)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
            }
        }
    }

    private void OnEnable()
    {
        referenceCollector = (ReferenceCollector) target;

        if (reorderableList == null)
        {
            reorderableList = new ReorderableList(referenceCollector.data, typeof (ReferenceCollectorData));
            reorderableList.drawHeaderCallback = (rect) => { GUI.Label(rect, "UI引用管理器"); };

            reorderableList.elementHeightCallback = i => reorderableList.elementHeight;

            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Color normal = GUI.color;

                // 当前元素
                ReferenceCollectorData cur = referenceCollector.data[index];
                Rect pos = rect;
                // 12格布局
                float gruid = pos.width / 12;
                var dataProperty = serializedObject.FindProperty("data");
                //================================================================================================================
                pos.width = 3 * gruid;
                pos.height = 16;
                pos.x += 1;
                pos.y += 1.5f;
                GUI.enabled = false;
                cur.key = EditorGUI.TextField(pos, cur.gameObject? cur.gameObject.name : cur.key);
                GUI.enabled = true;
                //================================================================================================================
                pos.x += pos.width + 2;
                pos.width = 5 * gruid;
                cur.gameObject = EditorGUI.ObjectField(pos, cur.gameObject, typeof (Object), true);

                //================================================================================================================
                pos.x += pos.width + 2;
                pos.width = 1 * gruid;
                if (cur.gameObject is GameObject)
                {
                    // 组件选择菜单
                    if (EditorGUI.DropdownButton(pos, new GUIContent(""), FocusType.Passive))
                    {
                        // create the menu and add items to it
                        GenericMenu menu = new GenericMenu();

                        // forward slashes nest menu items under submenus

                        //获取这个GameObject上的UI组件
                        if (cur.gameObject is GameObject)
                        {
                            // 获取所有UI组件
                            MonoBehaviour[] components =
                                    ((GameObject) cur.gameObject).GetComponents<MonoBehaviour>();
                            foreach (MonoBehaviour item in components)
                            {
                                AddMenuItem(menu, item.GetType().Name, new object[] { cur, item },
                                    OnUIBehaviourSelected);

                                //// an empty string will create a separator at the top level
                                //menu.AddSeparator("");

                                menu.ShowAsContext();
                            }
                        }
                    }

                    // 为了布局一致
                    pos.x += pos.width + 2;
                }
                else if (cur.gameObject is MonoBehaviour)
                {
                    GUI.color = Color.green;
                    if (GUI.Button(pos, "G"))
                    {
                        cur.gameObject = ((MonoBehaviour) cur.gameObject).gameObject;
                    }

                    GUI.color = normal;

                    // 为了布局一致
                    pos.x += pos.width + 2;
                }
                //================================================================================================================

                pos.width = gruid * 1.5f;
                GUI.color = cur.ignore? Color.gray : normal;
                if (GUI.Button(pos, "Ignore"))
                {
                    cur.ignore = !cur.ignore;
                }

                pos.x += pos.width + 2 + gruid * 0.2f;
                pos.width = gruid;
                GUI.color = Color.red;
                if (GUI.Button(pos, "X"))
                {
                    delList.Add(index);
                }

                foreach (var i in delList)
                {
                    dataProperty.DeleteArrayElementAtIndex(i);
                }

                delList.Clear();
                GUI.color = normal;

                //=====================================================  画数据绑定UI  =================================================

                GUI.color = normal;
            };
        }
    }

    /// <summary>
    /// 添加菜单项
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="menuPath"></param>
    /// <param name="selectItem"></param>
    /// <param name="menuFunction2"></param>
    private void AddMenuItem(GenericMenu menu, string menuPath, object selectItem, MenuFunction2 menuFunction2)
    {
        menu.AddItem(new GUIContent(menuPath), false, menuFunction2, selectItem);
    }

    /// <summary>
    /// UI组件被选中
    /// </summary>
    /// <param name="userData"></param>
    private void OnUIBehaviourSelected(object userData)
    {
        object[] args = (object[]) userData;
        ReferenceCollectorData cur = args[0] as ReferenceCollectorData;
        MonoBehaviour seleced = args[1] as MonoBehaviour;
        cur.gameObject = seleced;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10);

        Undo.RecordObject(referenceCollector, "Changed Settings");
        var dataProperty = serializedObject.FindProperty("data");

        TextAsset obj = null;
        if (!string.IsNullOrEmpty(referenceCollector.scriptGuid))
        {
            obj = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(referenceCollector.scriptGuid));
        }

        GUILayout.BeginHorizontal();
        var tempTextAsset = EditorGUILayout.ObjectField(obj, typeof (TextAsset), false);
        if (GUILayout.Button("R", GUILayout.Width(40)))
        {
            referenceCollector.FindUIScript();
        }

        GUILayout.EndHorizontal();
        if (tempTextAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(tempTextAsset);
            if (Path.GetExtension(path) != ".cs")
            {
                referenceCollector.scriptGuid = null;
            }
            else
            {
                referenceCollector.scriptGuid = AssetDatabase.AssetPathToGUID(path);
            }
        }

        GUILayout.BeginHorizontal("box");
        // if (GUILayout.Button("添加引用"))
        // {
        //     AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
        // }

        if (GUILayout.Button("全部删除"))
        {
            dataProperty.ClearArray();
        }

        if (GUILayout.Button("删除空引用"))
        {
            DelNullReference();
        }

        if (GUILayout.Button("排序"))
        {
            Sort();
        }

        // if (GUILayout.Button("生成字段"))
        // {
        //     GenFields();
        // }
        //
        // if (GUILayout.Button("生成取值"))
        // {
        //     GenGetCodes();
        // }

        if (GUILayout.Button("重置Key为名称"))
        {
            ReNameAll();
        }

        if (GUILayout.Button("●"))
        {
            GenPartialCodes();
        }

        EditorGUILayout.EndHorizontal();

        reorderableList.DoLayoutList();

        var eventType = Event.current.type;
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            // Show a copy icon on the drag
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var o in DragAndDrop.objectReferences)
                {
                    AddReference(dataProperty, o.name, o);
                }
            }

            Event.current.Use();
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();

        //Debug.Log(GUILayoutUtility.GetLastRect());
    }

    private void AddReference(SerializedProperty dataProperty, string key, Object obj)
    {
        int index = dataProperty.arraySize;
        dataProperty.InsertArrayElementAtIndex(index);
        var element = dataProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("key").stringValue = key;
        element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
    }

    public void Add(string key, Object obj)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < referenceCollector.data.Count; i++)
        {
            if (referenceCollector.data[i].key == key)
            {
                break;
            }
        }

        if (i != referenceCollector.data.Count)
        {
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        else
        {
            dataProperty.InsertArrayElementAtIndex(i);
            SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }

        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Remove(string key)
    {
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < referenceCollector.data.Count; i++)
        {
            if (referenceCollector.data[i].key == key)
            {
                break;
            }
        }

        if (i != referenceCollector.data.Count)
        {
            dataProperty.DeleteArrayElementAtIndex(i);
        }

        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Clear()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        var dataProperty = serializedObject.FindProperty("data");
        dataProperty.ClearArray();
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    public void Sort()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        referenceCollector.data.Sort(new ReferenceCollectorDataComparer());
        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }

    private void ReNameAll()
    {
        foreach (var item in referenceCollector.data)
        {
            item.key = item.gameObject.name;
        }
    }

    /// <summary>
    /// 生成字段Code
    /// </summary>
    private Tuple<string, string, string> GenerateCodes()
    {
        List<ReferenceCollectorData> list = referenceCollector.data;

        StringBuilder namespaceStringBuilder = new StringBuilder();
        StringBuilder fieldStringBuilder = new StringBuilder();
        StringBuilder codeStringBuilder = new StringBuilder();

        HashSet<string> namespaceHashSet = new HashSet<string>();

        foreach (ReferenceCollectorData item in list)
        {
            if (item.ignore)
                continue;

            var type = item.gameObject.GetType().Name;
            var fieldName = item.key;
            var namespaceStr = item.gameObject.GetType().Namespace;
            if (!namespaceHashSet.Contains(namespaceStr))
            {
                namespaceHashSet.Add(namespaceStr);
                namespaceStringBuilder.AppendFormat($"using {namespaceStr};\n");
            }

            fieldName = fieldName[0].ToString().ToLower() + fieldName.Substring(1);

            fieldStringBuilder.AppendFormat("\t\tprivate {0} _{1};\n", type, fieldName);
            codeStringBuilder.AppendFormat("\t\t\t_{0} = View.Get<{1}>(\"{2}\");\n", fieldName, type,
                item.key);
        }

        // this.ClipBoard(fieldStringBuilder.ToString());
        // Debug.Log("生成字段成功！");
        return new Tuple<string, string, string>(fieldStringBuilder.ToString(), codeStringBuilder.ToString(),
            namespaceStringBuilder.ToString());
    }

    /// <summary>
    /// 生成获取代码
    /// </summary>
    private string GenGetCodes()
    {
        List<ReferenceCollectorData> list = referenceCollector.data;

        StringBuilder template = new StringBuilder();

        string type = null;

        string fieldName = null;

        foreach (ReferenceCollectorData item in list)
        {
            type = item.gameObject.GetType().Name;

            fieldName = item.key;

            fieldName = fieldName[0].ToString().ToLower() + fieldName.Substring(1);

            template.AppendFormat("_{0} = View.Get<{1}>(\"{2}\");\n", fieldName, type, item.key);
        }

        this.ClipBoard(template.ToString());
        Debug.Log("生成获取代码成功！");
        return template.ToString();
    }

    /// <summary>
    /// 生成获取代码，在部分类中。
    /// </summary>
    private void GenPartialCodes()
    {
        // 如果没有脚本引用，就把代码生成到剪切板中。
        if (string.IsNullOrEmpty(referenceCollector.scriptGuid))
        {
            StringBuilder template = new StringBuilder();
            var codes = GenerateCodes();
            template.Append($@"
// 此代码是Editor自动生成的，请不要进行修改。

    public partial class Template
    {{
        // 字段 
{codes.Item1}
        protected void OnInit(GameObject View)
        {{
            // 获取引用
{codes.Item2}
        }}
    }}
");
            this.ClipBoard(template.ToString());
            return;
        }

        // Assets/Model/UI/LoadingPage.cs
        string genPath = AssetDatabase.GUIDToAssetPath(referenceCollector.scriptGuid);
        if (genPath != null)
        {
            string namespaceStr = CsFileUtils.GetNamespace(genPath);
            string className = this.referenceCollector.gameObject.name;
            StringBuilder template = new StringBuilder();
            var codes = GenerateCodes();
            template.Append($@"
// 此代码是Editor自动生成的，请不要进行修改。

using AkaUI;
{codes.Item3}
");

            if (string.IsNullOrEmpty(namespaceStr))
            {
                template.Append($@"
    public partial class {className}
    {{
        // 字段 
{codes.Item1}
        protected override void OnInit()
        {{
            base.OnInit();

            // 获取引用
{codes.Item2}
        }}
    }}
");
            }
            else
            {
                template.Append($@"
namespace {namespaceStr}
{{
    public partial class {className}
    {{
        // 字段 
{codes.Item1}
        protected override void OnInit()
        {{
            base.OnInit();

            // 获取引用
{codes.Item2}
        }}
    }}
}}
");
            }

            string output = Path.Combine(Path.GetDirectoryName(genPath) ?? "./Assets/", className + ".Gen.cs");
            File.WriteAllText(output, template.ToString());
            Debug.Log($"output -> {output}");
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 文本到剪切板
    /// </summary>
    /// <param name="str"></param>
    private void ClipBoard(string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
}