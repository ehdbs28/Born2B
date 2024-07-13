using System;

public interface IHealth
{
    public int CurrentHp { get; }
    public int MaxHp { get; }
    public int CanChangedHPCount { get; set; }
    public Action<int, int> OnChangedHpEvent { get; set; }
    public void ResetHp();
    public void AddHp(int healHp);
    public void ReduceHp(int reduceHp);
}