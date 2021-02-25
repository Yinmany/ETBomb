using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ET
{
    [CustomEditor(typeof (Bootstrap))]
    public class BootstrapEditor: Editor
    {
        private Bootstrap _self;

        private void OnEnable()
        {
            this._self = this.target as Bootstrap;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            this._self.assemblyNames.Clear();
            foreach (AssemblyDefinitionAsset assemblyDefinitionAsset in this._self.AssemblyDefinitionAssets)
            {
                this._self.assemblyNames.Add(assemblyDefinitionAsset.name + ".dll");
            }
        }
    }
}