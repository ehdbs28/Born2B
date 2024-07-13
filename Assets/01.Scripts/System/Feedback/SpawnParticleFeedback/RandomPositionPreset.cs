using System;
using UnityEngine;

public partial class SpawnParticleFeedback : Feedback
{
    [Serializable]
    public class RandomPositionPreset
    {
        public float Radius = 10f;

        [Header("Ignore Option")]
        public bool X;
        public bool Y;
        public bool Z;
    }
}
