﻿using System.Collections.Generic;

namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Senario item
    /// </summary>
    public class SenarioItem : ClassItem
    {

        public SenarioItem(string name, string codeNamespace, string fullname, SpecificationItem spItem) : base(name, codeNamespace, fullname)
        {
            Specification = spItem;
        }

        /// <summary>
        /// Unit tests
        /// </summary>
        public IEnumerable<TestItem> Tests { get; private set; }

        public SpecificationItem Specification { get; private set; }

    }
}