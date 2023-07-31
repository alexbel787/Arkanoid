using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public AudioSource soundSource;
	public AudioSource musicSource;					//Drag a reference to the audio source which will play the music.
	public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.				
	public float masterSoundVolume;
	public float masterMusicVolume;
	public float lowPitchRange = .95f;				//The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	public AudioClip[] soundtrack;
	public AudioClip[] musicSounds;

	[Header("Common Sounds")]
	public AudioClip[] gameSounds;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		LoadPlayerPrefs();
	}

    public void PlayMusic(float volMultiplier, AudioClip clip)
    {
		musicSource.volume = masterMusicVolume * volMultiplier;
		musicSource.clip = clip;
		musicSource.Play();
	}

	public void PlaySingle(float volMultiplier, AudioClip clip)
	{
		soundSource.pitch = 1f;
		soundSource.PlayOneShot(clip, masterSoundVolume * volMultiplier);
	}

	public void RandomizeSfx(float volMultiplier, params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		soundSource.pitch = randomPitch;
		soundSource.PlayOneShot(clips[randomIndex], masterSoundVolume * volMultiplier);
	}

	private void LoadPlayerPrefs()
	{
		masterSoundVolume = PlayerPrefs.GetFloat("soundVol", 1f);
		masterMusicVolume = PlayerPrefs.GetFloat("musicVol", 1f);
	}

	public void FadeOut(AudioSource audioSource, float fadeTime, float soundVolume)
    {
		StartCoroutine(FadeOutCotoutine(audioSource, fadeTime, soundVolume));
    }

	private IEnumerator FadeOutCotoutine(AudioSource audioSource, float fadeTime, float soundVolume)
	{
		while (audioSource.volume > 0)
		{
			audioSource.volume -= soundVolume * Time.deltaTime / fadeTime;

			yield return null;
		}

		audioSource.Stop();
		audioSource.volume = soundVolume;
	}

	public void FadeIn(AudioSource audioSource, float fadeTime, float soundVolume)
	{
		StartCoroutine(FadeInCoroutine(audioSource, fadeTime, soundVolume));
	}

	private IEnumerator FadeInCoroutine(AudioSource audioSource, float fadeTime, float soundVolume)
	{
		audioSource.volume = 0;
		audioSource.Play();
		while (audioSource.volume < soundVolume)
		{
			audioSource.volume += soundVolume * Time.deltaTime / fadeTime;
			yield return null;
		}
		audioSource.volume = soundVolume;
	}

}

