using UnityEngine;

public partial class SpawnParticleFeedback : Feedback
{
    [SerializeField] PoolableParticle particlePrefab = null;

    [SerializeField] bool multipleParticle = false;

    [ConditionalField("multipleParticle", true)]
    [SerializeField] MultipleParticlePreset multipleSetting = new MultipleParticlePreset();

    [SerializeField] bool randomPosition = false;
    [ConditionalField("randomPosition", true)]
    [SerializeField] RandomPositionPreset positionSetting = new RandomPositionPreset();

    [SerializeField] bool randomSize = false;
    [ConditionalField("randomSize", true)]
    [SerializeField] Vector2 range = new Vector2(0.5f, 1f);

    public override void Play(Vector3 playPos)
    {
        if (multipleParticle)
            SpawnMultipleParticle(playPos);
        else
            SpawnParticle(playPos);
    }

    private void SpawnParticle(Vector3 position)
    {
        PoolableParticle instance = PoolManager.Instance.Pop(particlePrefab.name) as PoolableParticle;

        instance.transform.position = position;
        if (randomPosition)
        {
            Vector3 randomPosition = Random.insideUnitSphere * positionSetting.Radius;
            if (positionSetting.X)
                randomPosition.x = 0f;
            if (positionSetting.Y)
                randomPosition.y = 0f;
            if (positionSetting.Z)
                randomPosition.z = 0f;
            instance.transform.position += randomPosition;
        }

        if (randomSize)
            instance.transform.localScale *= Random.Range(range.x, range.y);

        instance.Play();
    }

    private void SpawnMultipleParticle(Vector3 playPos)
    {
        foreach (var i in multipleSetting)
        {
            StartCoroutine(this.DelayCoroutine(i.Delay, () =>
            {
                int count = i.Count + Random.Range(-i.Randomness, i.Randomness);
                for (int j = 0; j < count; ++j)
                    SpawnParticle(playPos);
            }));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (randomPosition == false)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, positionSetting.Radius);
    }
#endif
}
