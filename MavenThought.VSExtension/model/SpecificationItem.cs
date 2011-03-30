using System.Collections.Generic;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Specification item
    /// </summary>
    public class SpecificationItem : ClassItem
    {
        public SpecificationItem(string name, string codeNamespace, string fullname) : base(name, codeNamespace, fullname)
        {
        }

        /// <summary>
        /// Senarios
        /// </summary>
        public IEnumerable<SenarioItem> Senarios { get; private set; }
    }
}