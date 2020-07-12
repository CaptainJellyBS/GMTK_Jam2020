using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public enum Track { Bass, Drums, Guitar }
public class AudioManager : MonoBehaviour
{
    [FMODUnity.EventRef] public string BassEvent;
    [FMODUnity.EventRef] public string DrumsEvent;
    [FMODUnity.EventRef] public string GuitarEvent;

    EventInstance bass;
    EventInstance drums;
    EventInstance guitar;

    private Dictionary<Track, EventInstance> tracks;
    private List<AudioEmitter> audioEmitters;

    public IEnumerator fadeCoroutine;
    private bool delayDone;
    public static AudioManager Instance { get; private set; } 

    private void Awake()
    {
        Instance = this;

        audioEmitters = new List<AudioEmitter>();
        audioEmitters.AddRange(FindObjectsOfType<AudioEmitter>());

        bass = FMODUnity.RuntimeManager.CreateInstance(BassEvent);
        drums = FMODUnity.RuntimeManager.CreateInstance(DrumsEvent);
        guitar = FMODUnity.RuntimeManager.CreateInstance(GuitarEvent);

        tracks = new Dictionary<Track, EventInstance>()
        { 
            {Track.Bass, bass}, 
            {Track.Drums, drums}, 
            {Track.Guitar, guitar}
        };

    }

    private IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (EventInstance i in tracks.Values)
        {
            i.start();
            i.setVolume(0.0f);
        }

        //fadeCoroutine = FadeInTrack(Track.Bass, 3.0f);
        //StartCoroutine(fadeCoroutine);
        tracks[Track.Bass].setVolume(1.0f);

        delayDone = true;
    }

    private void Start()
    {
        StartCoroutine("AudioDelay");
    }

    private void Update()
    {
        //change the guitar volume based on the distance between Dog and Soldier
        if (delayDone)
        {
            tracks[Track.Guitar].getVolume(out float firstVolume);
            float vol = (10 - Vector3.Distance(Dog.Instance.transform.position, Soldier.Instance.transform.position)) / 8;
            vol = Mathf.Clamp(vol, 0.0f, 1.0f);

            
            SetVolume(Track.Guitar, Mathf.Lerp(firstVolume,vol,0.1f));
        }

        //if drums are playing but no enemies are attacking, fade out drums
        if (GameHandler.Instance?.attackingEnemies == 0 && IsPlaying(Track.Drums))
        {
            fadeCoroutine = FadeOutTrack(Track.Drums, 0.5f);
            StartCoroutine(fadeCoroutine);
        }

        // if drums aren't playing yet (and hasn't been triggered by any other enemy), fade in drums
        
        if (GameHandler.Instance.attackingEnemies > 0 && !IsPlaying(Track.Drums))
        {
            fadeCoroutine = FadeInTrack(Track.Drums, 1.0f);
            StartCoroutine(fadeCoroutine);
        }
    }

    public bool IsPlaying(Track track)
    {
        EventInstance inst;
        tracks.TryGetValue(track, out inst);
        if (!inst.isValid() || !delayDone)
            return false;
        
        inst.getVolume(out float volume);

        if (track == Track.Guitar)
        {
            if (volume > 0)
                return true;
            else
                return false;
        }
        else
        {
            if (volume>=1.0f)
                return true;
            else
                return false;
        }
    }

    public void SetVolume(Track track, float volume)
    {
        EventInstance inst;
        tracks.TryGetValue(track, out inst);

        inst.setVolume(volume);
    }


    public IEnumerator FadeInTrack(Track track, float timeIn)
    {
        tracks.TryGetValue(track, out EventInstance inst);

        float volume;
        inst.getVolume(out volume);

        while (volume<1.0f)
        {
            inst.setVolume(volume + 0.01f / timeIn);
            inst.getVolume(out volume);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeOutTrack(Track track, float timeIn)
    {
        tracks.TryGetValue(track, out EventInstance inst);

        float volume;
        inst.getVolume(out volume);

        while (volume > 0)
        {
            inst.setVolume(volume - 0.01f / timeIn);
            inst.getVolume(out volume);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void StopAllMusic()
    {
        tracks[Track.Bass].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        tracks[Track.Guitar].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        tracks[Track.Drums].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopAllSounds()
    {
        foreach (AudioEmitter a in audioEmitters)
        {
            a.StopSound();
        }
    }

    private void OnApplicationQuit()
    {
        foreach (EventInstance i in tracks.Values)
        {
            i.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            i.release();
        }
    }
}
