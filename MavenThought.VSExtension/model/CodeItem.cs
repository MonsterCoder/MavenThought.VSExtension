using System;
using EnvDTE;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Class for a code item
    /// </summary>
    public abstract class CodeItem
    {
        protected CodeItem(string name, ProjectItem item)
        {
            Name = name;
            Item = item;
        }

        /// <summary>
        /// Code item name
        /// </summary>
        public string Name { get; private set; }

        public ProjectItem Item { get; private set; }
    }
}