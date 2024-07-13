using UnityEngine;

public class WeaponRangeTile : MonoBehaviour
{
    [SerializeField] Renderer tileRenderer = null;
    private Material material = null;

    private void Awake()
    {
        material = tileRenderer.material;
    }

    private void Start()
    {
        SetTileColor(0f);
    }

    public void SetTileColor(float alpha)
    {
        material.SetFloat("_Alpha", alpha);
    }
}
