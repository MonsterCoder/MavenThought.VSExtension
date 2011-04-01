using System;
using GeorgeChen.MavenThought_VSExtension.model;

namespace GeorgeChen.MavenThought_VSExtension.command
{
    public class SpecificationEventArgs : EventArgs
    {
        public ClassItem Source { get; set; }

        public SpecificationEventArgs(ClassItem source)
        {
            Source = source;
        }
    }
}