using System;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Class for a code item
    /// </summary>
    public abstract class CodeItem
    {
        protected CodeItem(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Code item name
        /// </summary>
        public string Name { get; private set; }        
    }
}