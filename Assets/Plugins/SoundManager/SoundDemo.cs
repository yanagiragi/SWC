using UnityEngine;
using System.Collections;

public class SoundDemo : MonoBehaviour {
	void Start () {}
	void Update () {
//		撥放聲音
		SoundManager.Play("Table","Sound", 1, false);
//		p_table  string 音效表名
//		p_key  string 音效名
//		p_volume float (可選)次音量(0~1)，真正的撥出音量會再乘上配置的主音量
//		p_loop  bool (可選)是否循環
				
//		停止聲音
		SoundManager.Stop("Table","Sound");
//		p_table  string 音效表名
//		p_key  string 音效名

//		停止特定物件聲音
		SoundPlayObj _Obj = SoundManager.Play("Table","Sound");
		SoundManager.StopSoundPlayObj(_Obj);

//		調整一個配置的主音量
		SoundManager.SetVolumeByConfigName ("Config", 1);
//		p_configName  string 音效配置名
//		p_volume float 音量(0~1)

//		開關一個配置的靜音狀態
		SoundManager.SetMuteByConfigName ("Config", true);
//		p_configName  string 音效配置名
//		p_mute bool 是否靜音
	}


}
