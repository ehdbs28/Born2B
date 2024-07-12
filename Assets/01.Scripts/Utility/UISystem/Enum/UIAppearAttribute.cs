using System;

[Flags]
public enum UIAppearAttribute
{
    None = 0,
    ResetPosX = 1,
    ResetPosY = 2,
    ResetPos = ResetPosX | ResetPosY,
    UseAnimation = 4,
}