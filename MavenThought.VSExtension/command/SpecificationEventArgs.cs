using System;
using EnvDTE;
using GeorgeChen.MavenThought_VSExtension.model;

namespace GeorgeChen.MavenThought_VSExtension.command
{
    public class SpecificationEventArgs : EventArgs
    {
        public SpecificationEventArgs(ClassItem source, string specName)
        {
            Source = source;
            SpecName = specName;
        }

        public ClassItem Source { get; private set; }
        public string SpecName { get; private set; }
        public Project targetProject { get; set; }

    }
}