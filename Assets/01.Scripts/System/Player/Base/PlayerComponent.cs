using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
	protected PlayerInstance player = null;

    private void Awake() {}
    private void Start() {}

	public virtual void Init(PlayerInstance player)
    {
        this.player = player;
    }

    public virtual void Release()
    {
        
    }
}
