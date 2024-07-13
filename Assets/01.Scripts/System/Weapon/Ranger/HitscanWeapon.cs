/// <summary>
/// 그냥 무조건 1의 데미지 계수를 반환
/// </summary>
public class HitscanWeapon : RangeWeapon
{ 
    protected override float ProcessTheta(float theta)
    {
        return 1f;
    }
}
