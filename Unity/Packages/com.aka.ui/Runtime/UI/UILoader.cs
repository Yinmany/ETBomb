using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace AkaUI
{
    public struct UILoader
    {
        public UIBase UI { get; set; }
        public Task<GameObject> Task { get; set; }

        public TaskAwaiter<GameObject> GetAwaiter()
        {
            return Task.GetAwaiter();
        }

        public void OnCompleted(Action action)
        {
            GetAwaiter().OnCompleted(action);
        }
    }
}