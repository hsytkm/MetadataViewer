using System;

namespace MetadataViewer.Models
{
    static class AssemblyState
    {
        public const bool IsDebugBuild =
#if DEBUG
            true;
#else
            false;
#endif
    }
}
