using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBGMRoller : MonoBehaviour
{
    [SerializeField] private AudioData data;

    private void Start()
    {
        AudioManager.Instance.PlayAudio(data);
    }
}
