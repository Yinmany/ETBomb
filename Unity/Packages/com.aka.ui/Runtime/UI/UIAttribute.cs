using System;

namespace AkaUI
{
    /// <summary>
    /// UI资源特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UIAttribute: Attribute
    {
        /// <summary>
        ///  预先就要加载的UI
        /// </summary>
        public bool Preload { get; set; }
    }
}