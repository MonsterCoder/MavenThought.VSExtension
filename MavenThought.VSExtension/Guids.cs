// Guids.cs
// MUST match guids.h
using System;

namespace GeorgeChen.MavenThought_VSExtension
{
    static class GuidList
    {
        public const string guidMavenThought_VSExtensionPkgString = "39a5b529-0eae-4ce2-8502-86e2a3863b13";
        public const string guidMavenThought_VSExtensionCmdSetString = "c3693190-1f65-4ee9-a316-497ec52f3878";
        public const string guidToolWindowPersistanceString = "b83c1de4-8a32-4691-863b-7f7bb8ed96da";

        public static readonly Guid guidMavenThought_VSExtensionCmdSet = new Guid(guidMavenThought_VSExtensionCmdSetString);
    };
}