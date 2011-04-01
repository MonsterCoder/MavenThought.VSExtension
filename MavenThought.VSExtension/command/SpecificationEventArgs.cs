using System;
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

        public ClassItem Source { get; set; }
        public string SpecName { get; set; }
    }
}