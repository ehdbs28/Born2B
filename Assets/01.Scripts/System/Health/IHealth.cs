public interface IHealth
{
    public int CurrentHp { get; }
    public int MaxHp { get; }

    public void ResetHp();
    public void AddHp(int healHp);
    public void ReduceHp(int reduceHp);
}