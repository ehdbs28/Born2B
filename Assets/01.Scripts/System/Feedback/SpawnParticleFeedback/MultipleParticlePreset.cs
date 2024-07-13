using System;
using System.Collections;
using System.Collections.Generic;

public partial class SpawnParticleFeedback
{
    [Serializable]
    public class MultipleParticlePreset : IEnumerable<MultipleParticleParams>
    {
        public List<MultipleParticleParams> Settings = new List<MultipleParticleParams>();
        public MultipleParticleParams this[int index] => Settings[index];

        public IEnumerator<MultipleParticleParams> GetEnumerator() => Settings.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Settings.GetEnumerator();
    }

    [Serializable]
    public class MultipleParticleParams
    {
        public float Delay = 0f;
        public int Count = 5;
        public int Randomness = 2;
    }
}
