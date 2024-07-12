using UnityEngine;

public struct AttackParams
{
    public float attack;
    public float criticalChance;
    public float criticalDamage;

    public AttackParams(float attack, float criticalChance, float criticalDamage)
    {
        this.attack = attack;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
    }

    public bool ProcessCritical()
    {
        float delta = Random.Range(0f, 100f);
        return delta < criticalChance;
    }
}
