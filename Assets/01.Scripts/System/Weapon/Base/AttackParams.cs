using UnityEngine;

public struct AttackParams
{
    public float attack;
    public float criticalChance;
    public float criticalDamage;
    public LayerMask targetLayer;

    public AttackParams(float attack, float criticalChance, float criticalDamage, LayerMask targetLayer)
    {
        this.attack = attack;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.targetLayer = targetLayer;
    }

    public bool ProcessCritical()
    {
        float delta = Random.Range(0f, 100f);
        return delta < criticalChance;
    }
}
