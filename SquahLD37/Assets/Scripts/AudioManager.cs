using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//All available sound effects enumerated. Actual sound effects are saved in AudioManager.audios
public enum Audio {
    Death,
	GetHit,
	Swing,
	SwingHit,
	WallHit,
	MonsterAttack
}

/// <summary>
/// Manages all audio (background music, sound effect and dialogue playback)
/// </summary>
public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

    //We alternate between two base sources to fade in and out of background tracks. During normal play, one of these has a volume of 100%, while the other has a volume of 0%
	public AudioSource[] baseSources;
    public float fadeTimer = 0;
    public bool fadeOut = false;
    public bool fadeIn = false;
    public bool isFading = false;
    public float fadeSpeed = 5;
	public float dampenVolume = 0.25f;

	private AudioSource fadeFrom;
	private AudioSource fadeTo;
	private AudioSource current;

    //Actual clips for sound effects and background music, filled in the inspector of GameManager in Managers scene
    public List<AudioClip> audios;
    //public List<AudioClip> BGs;

    //Using an object pool for Audio Sources so the components can be reused
    private List<AudioSource> sources = new List<AudioSource>();
    private List<AudioSource> playingSources = new List<AudioSource>();

    private AudioSource specialSound;
    public List<AudioClip> specialSoundsPlayed = new List<AudioClip>();
    private bool isDampened;
    public bool lastLevel;

    private AudioSource voiceoverSound;

    private static bool mute;

	private bool forceDampen;

	//Start playing the background music
	public void Awake ()
	{
		if (instance)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(gameObject);

		foreach (AudioSource baseSource in baseSources)
			baseSource.Play();
		current = baseSources[0];
	}

    void Update()
    {
        //Follow the camera so audio is always played at 100%
        if (Camera.main != null)
            transform.position = Camera.main.transform.position;

        //Place audiosources back into the object pool if they are done playing
        List<AudioSource> moveSources = new List<AudioSource>();
        foreach (AudioSource source in playingSources)
        {
            if (!source.isPlaying)
            {
                moveSources.Add(source);
                sources.Add(source);
            }
        }
        foreach (AudioSource source in moveSources)
        {
            playingSources.Remove(source);
        }

	    DoFade();

        if (lastLevel && isDampened && SpecialSoundComplete())
            UndampenMusic();
    }

	private void DoFade()
	{
		if (!isFading || forceDampen)
			return;

		if (fadeFrom == fadeTo)
			return;

		fadeTimer -= Time.deltaTime;
		fadeFrom.volume = fadeTimer;
		fadeTo.volume = 1 - fadeTimer;

		if (fadeTimer <= 0)
		{
			fadeTimer = 0;
			isFading = false;
			current = fadeTo;
		}
	}

    /// <summary>
    /// Start fading the current background track out, and prepare baseSource2 for fadeIn
    /// </summary>
    /// <param name="trackIndex">The index of the next background track</param>
    public void SwitchBase(int trackIndex)
    {
        fadeTimer = 1;
        isFading = true;
	    fadeFrom = current;
	    fadeTo = baseSources[trackIndex];
    }

    /// <summary>
    /// Start playing background music on a given source
    /// </summary>
    /// <param name="source">The source to play music (baseSource or baseSource2)</param>
    /// <param name="on"></param>
    /// <param name="volume"></param>
    public void PlayMusic(AudioSource source)
    {
        if (!source.isPlaying && !mute)
        {
            source.Play();
        }

    }

    /// <summary>
    /// Play a sound effect
    /// </summary>
    /// <param name="audio">The enumerated sound effect name</param>
    /// <param name="volume">Playback volume</param>
    public AudioSource PlaySound(Audio audio, float volume = 1)
    {
	    if (audios[(int) audio] == null)
		    return null;
        return PlaySound(audios[(int) audio], volume);
    }

    

    /// <summary>
    /// Play a sound effect
    /// </summary>
    /// <param name="clip">The actual clip to play</param>
    /// <param name="volume">Playback volume</param>
    public AudioSource PlaySound(AudioClip clip, float volume = 1)
    {
	    var source = getSource();
        if (mute)
            volume = 0;
	    source.PlayOneShot(clip, volume);
        return source;
    }

    public bool PlaySpecialSound(AudioClip clip, float volume = 1)
    {
        if (!specialSoundsPlayed.Contains(clip))
        {
            lastLevel = true;
            DampenMusic();
            specialSound = PlaySound(clip, volume);
            specialSoundsPlayed.Add(clip);
            return true;
        }
        return false;
    }

    public void PlayVoiceoverSound(AudioClip clip, float volume = 1)
    {
        voiceoverSound = PlaySound(clip, volume);
    }

    public void SkipVoiceover()
    {
        if (voiceoverSound)
            voiceoverSound.Stop();
    }

    public void Mute()
    {
        mute = true;
	    foreach (AudioSource baseSource in baseSources)
		    baseSource.volume = 0;
		//TODO something with stop fading
    }

    public void Unmute()
    {
        mute = false;
	    current.volume = 1;
    }

    public bool SpecialSoundComplete()
    {
        return (!specialSound || !specialSound.isPlaying);
    }

    public bool VoiceoverComplete()
    {
        return (!voiceoverSound || !voiceoverSound.isPlaying);
    }

    /// <summary>
    /// Ask the object pool for an available audiosource, or a new one if none are available
    /// </summary>
    /// <returns>A ready-to-use AudioSource</returns>
    private AudioSource getSource()
    {
        //If there is at least one source available, start using the first one
        if (sources.Count > 0)
        {
            AudioSource a = sources[0];
            playingSources.Add(a);
            sources.Remove(a);
            return a;
        }
        //Otherwise, create a new AudioSource
        else
        {
            AudioSource a = instance.gameObject.AddComponent<AudioSource>();
            playingSources.Add(a);
            return a;
        }
    }

    /// <summary>
    /// Dampen the background music so dialogue is easier to hear
    /// </summary>
    public void DampenMusic() {
        if (!isFading)
        {
	        current.volume = dampenVolume;
	        forceDampen = true;
            isDampened = true;
        }
    }

    /// <summary>
    /// Undampen the background music
    /// </summary>
    public void UndampenMusic() {
        if (!isFading)
        {
            current.volume = 1f;
	        forceDampen = false;
            isDampened = false;
        }
    }

    /// <summary>
    /// Check if the given levelIndex corresponds to a levelIndex where the background track should be changed. If it does, start changing the background music
    /// </summary>
    /// <param name="levelIndex">levelIndex that might correspond to an audioSwitchLevel</param>
    public void FadeBaseOut(int levelIndex)
    {
		SwitchBase(levelIndex);
    }

    /// <summary>
    /// Start fading the new background music back in. Only has an effect if the previous background track was already faded out.
    /// </summary>
    public void FadeBaseIn()
    {
        fadeIn = true;
    }
}
