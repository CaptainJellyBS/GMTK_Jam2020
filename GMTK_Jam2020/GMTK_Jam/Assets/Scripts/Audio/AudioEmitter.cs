using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    [FMODUnity.EventRef] public string SoundEvent = "";

    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool repeat = true;
    [SerializeField] [Tooltip("How long until this audio repeats? (in seconds)")] private float repeatInterval = 0.5f;
    [SerializeField] [Tooltip("Should this audio be allowed to fade out when stopped?")] private bool allowFadeOut = false;

    

    private bool coroutineRunning;

    FMOD.Studio.EventInstance sound;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        sound = FMODUnity.RuntimeManager.CreateInstance(SoundEvent);

        //depending on settings, play audio once or repeat
        if (playOnAwake)
        {
            PlaySound();
        }
    }

    //play sound every repeatInterval
    private IEnumerator PlaySoundRepeat()
    {
        while (true)
        {
            coroutineRunning = true;
            PLAYBACK_STATE isPlaying;
            sound.getPlaybackState(out isPlaying);


            if(isPlaying == PLAYBACK_STATE.PLAYING)
            {
                sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            sound.start();
            yield return new WaitForSeconds(repeatInterval);
        }
    }

    void Update()
    {
        Set3DAttributes();
    }

    public void PlaySound()
    {
        
        if (!repeat)
        {
            sound.start();
        }
        else
        {
            StartCoroutine("PlaySoundRepeat");
        }
    }

    public void StopSound()
    {
        if (coroutineRunning)
        {
            StopCoroutine("PlaySoundRepeat");
        }

        if (allowFadeOut)
        {
            sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    //set the 3D attributes of the audio event, checking for a rigidbody in the process
    private void Set3DAttributes()
    {
        if (rigidBody==null)
        {
            sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            return;
        }
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, rigidBody));
    }

    //fade or hard stop audio depending on settings
    private void OnDestroy()
    {
        StopSound();
        sound.release();
    }
}
