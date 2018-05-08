using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : InstanceObject<SoundManager> {
	
	[SerializeField] private List<SoundData> bgmSoundsData;
	[SerializeField] private List<SoundData> soundsData;

	void Awake()
	{
		base.Awake();
		PlaySoundBGM("BGM", 0.5f);
	}

	public void PlaySoundBGM(string name, float volume = 1)
	{
		AudioClip ac = bgmSoundsData.Find(soundData => name == soundData.soundName).clip;
		if(ac != null)
		{
			GameObject obj = new GameObject();
			obj.name = "PlayBGMAudio_" + ac.name;
			AudioSource audio = obj.AddComponent<AudioSource>();
			audio.clip = ac;
			audio.volume = volume;
			audio.Play();
			audio.loop = true;
		}
	}

	public void PlaySound(string name)
	{
		AudioClip ac = soundsData.Find(soundData => name == soundData.soundName).clip;
		if(ac != null)
		{
			GameObject obj = new GameObject();
			obj.name = "PlayAudio_" + ac.name;
			AudioSource audio = obj.AddComponent<AudioSource>();
			audio.clip = ac;
			audio.Play();
			Destroy(obj, audio.clip.length);
		}
	}

	public void PlaySound(string name, out AudioSource audio)
	{
        AudioClip ac = soundsData.Find(soundData => name == soundData.soundName).clip;
        if (ac != null)
        {
            GameObject obj = new GameObject();
            obj.name = "PlayAudio_" + ac.name;
            audio = obj.AddComponent<AudioSource>();
            audio.clip = ac;

            audio.Play();
            Destroy(obj, audio.clip.length);
        }
		else
			audio = null;
	}

}