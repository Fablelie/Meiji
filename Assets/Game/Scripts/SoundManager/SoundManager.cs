using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : InstanceObject<SoundManager> {
	
	[SerializeField] private List<SoundData> bgmSoundsData;
	[SerializeField] private List<SoundData> soundsData;

	public List<AudioSource> BGMOnPlay;
	public List<AudioSource> BGMOnPause;

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
			BGMOnPlay.Add(audio);
		}
	}

	public void PauseALLBGM()
	{
		for(int i = 0;i < BGMOnPlay.Count; i++)
		{
			// Debug.LogError("pause : " +BGMOnPlay[i].name);
			BGMOnPlay[i].Pause();
			BGMOnPause.Add(BGMOnPlay[i]);
			BGMOnPlay.Remove(BGMOnPlay[i]);
		}
	}

	public void UnPauseAllBGM()
	{
		for(int i = 0;i < BGMOnPause.Count; i++)
		{
			// Debug.LogError("unpause : "+BGMOnPause[i].name);
			BGMOnPause[i].UnPause();
			BGMOnPlay.Add(BGMOnPause[i]);
			BGMOnPause.Remove(BGMOnPause[i]);
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