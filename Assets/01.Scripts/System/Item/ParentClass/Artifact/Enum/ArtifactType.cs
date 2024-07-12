using System;

[Flags]
public enum ArtifactType
{
    None = 0,
    UseImmediately = 1 << 0,
    Usable = 1 << 1,
    CallByEvent = 1 << 2,
    Attributed = 1 << 3
}