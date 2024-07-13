using UnityEngine;

/// <summary>
/// theta를 확률로 두어 맞았는지 아닌지를 확인해 1 또는 0의 데미지 계수를 반환
/// </summary>
public class MultishotWeapon : RangeWeapon
{
    protected override float ProcessTheta(float theta)
    {
        bool hit = Random.Range(0, 100) < theta;
        return hit ? 1f : 0f;
    }
}
