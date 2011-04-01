using System.Collections.Generic;
using EnvDTE;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Specification item
    /// </summary>
    public class SpecificationItem : ClassItem
    {
        public SpecificationItem(string name, string codeNamespace, string fullname, ProjectItem item)
            : base(name, codeNamespace, fullname, item)
        {
        }

        /// <summary>
        /// Senarios
        /// </summary>
        public IEnumerable<SenarioItem> Senarios { get; private set; }
    }
}