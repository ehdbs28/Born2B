
[System.Flags]
public enum StatusType
{
    None = 0,
    Burn = 1 << 0,
    Frozen = 1 << 1,
    Sturned = 1 << 2,
    Poison = 1 << 3,
    Silence = 1 << 4,
    Drained = 1 << 5,
    Berserk = 1 << 6
}