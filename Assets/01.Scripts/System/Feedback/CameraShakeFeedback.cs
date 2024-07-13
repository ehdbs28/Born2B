using UnityEngine;

public class CameraShakeFeedback : Feedback
{
    [SerializeField] private float _intensity;
    
    public override void Play(Vector3 playPos)
    {
        CameraManager.Instance.ShakeCam(_intensity);
    }
}