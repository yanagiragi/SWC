using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayObj : MonoBehaviour {
	public SoundData soundData;
	public bool isPlaying = false;
	public int id = -1;
	public float volume = 1;
	AudioSource audio;
	void Awake () {
		if (audio == null) {
			audio = gameObject.GetComponent<AudioSource>();
		}
		if (audio == null) {
			audio = gameObject.AddComponent<AudioSource>();
		}
	}
	void Start () {}
	void Update () {
		isPlaying = audio.isPlaying;
		if (!isPlaying) {
			SoundManager.StopSoundPlayObj(this);
		}
	}

	public void Play (SoundData p_soundData,float p_volume,bool p_loop) {
		soundData = p_soundData;
		audio.clip = p_soundData.clip;
		if (p_soundData.table.config.mute) {
			SetVolume (p_volume, 0);
		} else {
			SetVolume (p_volume, p_soundData.table.config.adjustVolume);
		}
		audio.loop = p_loop;
		audio.Play ();
	}
	public void Stop (){
		audio.Stop ();
	}
	public void SetVolume (float p_volume,float p_adjustVolume){
		volume = p_volume;
		audio.volume = p_volume * p_adjustVolume;
	}
}
