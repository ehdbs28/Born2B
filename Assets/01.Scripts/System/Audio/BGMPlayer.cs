using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : AudioPlayer
{
    [SerializeField] private List<string> bgmList;
    [SerializeField] private float replayDelay;
    [SerializeField] private bool playOnAwake;
    private AudioClip currentBgm;

    private void Start()
    {
        if (playOnAwake)
            StartBGM();
    }

    public void StartBGM()
    {
        StopAllCoroutines();
        StartCoroutine(PlayBGM());
    }

    private IEnumerator PlayBGM()
    {
        while (true)
        {
            string bgmName = bgmList.PickRandom();
            currentBgm = audioLibrary[bgmName];
            PlayAudio(bgmName);

            yield return new WaitForSeconds(currentBgm.length + replayDelay);
        }
    }
}