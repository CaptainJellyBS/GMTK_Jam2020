using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Track { Bass, Drums, Guitar }
public class AudioManager : MonoBehaviour
{
    [FMODUnity.EventRef] public string BassEvent;
    [FMODUnity.EventRef] public string DrumsEvent;
    [FMODUnity.EventRef] public string GuitarEvent;

    FMOD.Studio.EventInstance bass;
    FMOD.Studio.EventInstance drums;
    FMOD.Studio.EventInstance guitar;

    private Dictionary<Track, FMOD.Studio.EventInstance> tracks;
    //private List<float> maxVolumes;

    private IEnumerator fadeIn;

    public static AudioManager Instance { get; private set; } 

    private void Awake()
    {
        Instance = this;

        bass = FMODUnity.RuntimeManager.CreateInstance(BassEvent);
        drums = FMODUnity.RuntimeManager.CreateInstance(DrumsEvent);
        guitar = FMODUnity.RuntimeManager.CreateInstance(GuitarEvent);

        tracks = new Dictionary<Track, FMOD.Studio.EventInstance>()
        { 
            {Track.Bass, bass}, 
            {Track.Drums, drums}, 
            {Track.Guitar, guitar}
        };

    }

    private void Start()
    {
        foreach (FMOD.Studio.EventInstance i in tracks.Values)
        {
            i.setVolume(0.0f);
            i.start();
        }

        fadeIn = FadeInTrack(Track.Bass, 3.0f);
        StartCoroutine(fadeIn);

        fadeIn = FadeInTrack(Track.Drums, 3.0f);
        StartCoroutine(fadeIn);
    }

    private void Update()
    {
        
        float vol = (12 - Vector3.Distance(Dog.Instance.transform.position, Soldier.Instance.transform.position)) / 10;
        
        vol = Mathf.Clamp(vol, 0.0f, 1.0f);
        SetVolume(Track.Guitar, vol);
    }

    public void SetVolume(Track track, float volume)
    {
        FMOD.Studio.EventInstance inst;
        tracks.TryGetValue(track, out inst);

        inst.setVolume(volume);
    }


    public IEnumerator FadeInTrack(Track track, float timeIn)
    {
        FMOD.Studio.EventInstance inst;
        tracks.TryGetValue(track, out inst);

        float volume;
        inst.getVolume(out volume);

        Debug.Log("volume: "+ volume);
        
        while (volume<1.0f)
        {
            inst.setVolume(volume + 0.01f / timeIn);
            inst.getVolume(out volume);

            yield return new WaitForSeconds(0.01f);
            Debug.Log("new volume: " + volume);
        }
    }

    public IEnumerator FadeOutTrack(Track track, float timeIn)
    {
        FMOD.Studio.EventInstance inst;
        tracks.TryGetValue(track, out inst);

        float volume;
        inst.getVolume(out volume);

        while (volume > 0.0f)
        {
            inst.setVolume(volume - 0.01f / timeIn);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnApplicationQuit()
    {
        foreach (FMOD.Studio.EventInstance i in tracks.Values)
        {
            i.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            i.release();
        }
    }
}
