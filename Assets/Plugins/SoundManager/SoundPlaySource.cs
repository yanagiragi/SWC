using UnityEngine;
using System.Collections;

public class SoundPlaySource : MonoBehaviour {
	[LockInInspector]public SoundPlayObj soundPlayObj;
	public string table = "";
	void Start () {
	
	}
	void Update () {
	
	}

	public void PlayByDefaultTable(string p_key){
		soundPlayObj = SoundManager.Play (table, p_key);
	}
//
//	public void Play(string p_table, string p_key){
//		soundPlayObj = SoundManager.Play (p_table, p_key);
//	}
//	public void Play(string p_table, string p_key, bool p_loop){
//		soundPlayObj = SoundManager.Play (p_table, p_key, p_loop);
//	}
//	public void Play(string p_table, string p_key, float p_volume){
//		soundPlayObj = SoundManager.Play (p_table, p_key, p_volume);
//	}
//	public void Play(string p_table, string p_key, float p_volume,bool p_loop){
//		soundPlayObj = SoundManager.Play (p_table, p_key, p_volume);
//	}
}
