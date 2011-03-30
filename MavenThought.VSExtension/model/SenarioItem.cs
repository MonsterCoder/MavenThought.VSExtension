using System.Collections.Generic;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Senario item
    /// </summary>
    public class SenarioItem : ClassItem
    {
        public SenarioItem(string name, string codeNamespace, string fullname) : base(name, codeNamespace, fullname)
        {
        }

        /// <summary>
        /// Unit tests
        /// </summary>
        public IEnumerable<TestItem> Tests { get; private set; }
    }
}