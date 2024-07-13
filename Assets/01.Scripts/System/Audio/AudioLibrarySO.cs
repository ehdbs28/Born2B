using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Audio/AudioLibrary")]
public class AudioLibrarySO : ScriptableObject
{
    [SerializeField] List<AudioDictionaryTable> audioTables = new List<AudioDictionaryTable>();
    public Dictionary<string, AudioDictionaryTable> Library = null;
    public AudioClip this[string key] => Library[key].Audio;

    private void OnEnable()
    {
        RefreshAudioAsset();
    }

    private void OnValidate()
    {
        RefreshAudioAsset();
    }

    [ContextMenu("Refresh")]
    private void Refresh()
    {
        RefreshAudioAsset();
    }

    private void RefreshAudioAsset()
    {
        if (Library != null)
            Library.Clear();
        else
            Library = new Dictionary<string, AudioDictionaryTable>();

        for (int i = 0; i < audioTables.Count; ++i)
        {
            AudioDictionaryTable table = audioTables[i];
            if (Library.ContainsKey(table.Key))
                continue;

            Library.Add(table.Key, table);
        }
    }

    public AudioDictionaryTable GetRandomAudio() => audioTables.PickRandom();
}