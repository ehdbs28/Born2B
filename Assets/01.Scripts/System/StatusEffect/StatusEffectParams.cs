public struct StatusEffectParams
{
    public StatusType statusType;
    public int turnCount;

    public StatusEffectParams(StatusType statusType, int turnCount)
    {
        this.statusType = statusType;
        this.turnCount = turnCount;
    }
}
