namespace Crostini
{
    public readonly struct Version
    {
        public struct Version
        {
            public int Major;
            public int Minor;
            public int Patch;
            public int hotfix;
            public int build;
        }
    }
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public int Hotfix { get; }
        public int Build { get; } 
        public Version(int major, int minor, int patch, int hotfix = 0, int build = 0)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Hotfix = hotfix;
            Build = build;
        }

        public override string ToString() => $"{Major}.{Minor}.{Patch}.{Hotfix}.{Build}";
    }
}
