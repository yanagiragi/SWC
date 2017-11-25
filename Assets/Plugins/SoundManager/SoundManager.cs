using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : ManagerBase<SoundManager> {
	static Dictionary<string, SoundTable> tableDict = new Dictionary<string, SoundTable>();
	static Dictionary<string, List<SoundTable>> tableInConfigDict = new Dictionary<string, List<SoundTable>>();
	static Dictionary<string, SoundConfigTable> configDict = new Dictionary<string, SoundConfigTable>();
	static Dictionary<int, SoundPlayObj> soundObjPlayingDict = new Dictionary<int, SoundPlayObj>();
	static Queue<SoundPlayObj> soundObjPoolList = new Queue<SoundPlayObj>();

	void Start () {}
	void Update () {}

#region "Data"
	public static SoundData GetSoundData(string p_tableName, string p_soundKey){
		SoundData _soundData = null;

		SoundTable _table = GetTable (p_tableName);
		if (_table != null) {
			_soundData = _table.GetSoundData(p_soundKey);
		}

		return _soundData;
	}
//	[Button("GetTable")] public string getTableBut = ""; 
	public static SoundTable GetTable(string p_name){
		SoundTable _table = null;
		if (!tableDict.TryGetValue (p_name, out _table)) {
			string _path = "SoundTable/" + p_name;
			_table = Resources.Load<SoundTable>(_path);
			if(_table != null){
				string _configName = _table.config.name;
				if(!configDict.ContainsKey(_configName)){
					GetConfig(_configName);
				}

				List<SoundTable> _tableList = _table.config.tableList;
				if (!tableInConfigDict.ContainsKey (_configName)) {
					tableInConfigDict.Add(_configName,_tableList);
				}

				Debug.Log("Load Sound Table [" + _table.name + "]");
				tableDict.Add(p_name,_table);

				_tableList.Add(_table);
				_table.Init();
			}else{
				Debug.LogError("No Sound Table : [Resources/" + _path + "]");
			}
		}

		return _table;
	}
	public static SoundConfigTable GetConfig(string p_configName){
		SoundConfigTable _config = null;
		if (!configDict.TryGetValue (p_configName, out _config)) {
			string _path = "SoundTable/" + p_configName;
			_config = Resources.Load<SoundConfigTable>(_path);
			if(_config != null){
				Debug.Log("Load Sound Config [" + _config.name + "]");
				_config.tableList.Clear();
				configDict.Add(p_configName,_config);
			}else{
				Debug.LogError("No Sound Config : [Resources/" + _path + "]");
			}
		}
		
		return _config;
	}

	public static void SetVolumeByConfigName(string p_configName,float p_volume){
		SoundConfigTable _config = GetConfig(p_configName);
		_config.adjustVolume = p_volume;
	}

	public static void SetMuteByConfigName(string p_configName,bool p_mute){
		SoundConfigTable _config = GetConfig(p_configName);
		_config.mute = p_mute;
	}

	static int tempId = 0;
	public static SoundPlayObj GetSoundPlayObj(){
		SoundPlayObj _obj = null;
		try{
			while(_obj == null){
				_obj = soundObjPoolList.Dequeue();
			}
		}catch(InvalidOperationException){}

		if(_obj == null){
			_obj = new GameObject().AddComponent<SoundPlayObj>();
			_obj.transform.SetParent(instance.transform);
		}
		_obj.gameObject.SetActive (true);
		_obj.id = tempId;
		_obj.name = "Sound Obj " + tempId;

		soundObjPlayingDict.Add (tempId,_obj);

		tempId++;
		return _obj;
	}
#endregion

#region "Play"
	public static SoundPlayObj Play(string p_table, string p_key){
		SoundData _soundData = GetSoundData (p_table,p_key);
		return _soundData.Play (_soundData.defaultVolume, _soundData.defaultLoop);
	}
	public static SoundPlayObj Play(string p_table, string p_key, bool p_loop){
		SoundData _soundData = GetSoundData (p_table,p_key);
		return _soundData.Play (_soundData.defaultVolume, p_loop);
	}
	public static SoundPlayObj Play(string p_table, string p_key, float p_volume){
		SoundData _soundData = GetSoundData (p_table,p_key);
		return _soundData.Play (p_volume, _soundData.defaultLoop);
	}
	public static SoundPlayObj Play(string p_table, string p_key, float p_volume,bool p_loop){
		SoundData _soundData = GetSoundData (p_table,p_key);
		return _soundData.Play (p_volume, p_loop);
	}

	public static void Stop(string p_table, string p_key){
		SoundData _soundData = GetSoundData (p_table,p_key);
		_soundData.Stop ();
	}

	public static void StopSoundPlayObj(SoundPlayObj _obj){
		soundObjPlayingDict.Remove (_obj.id);
		_obj.soundData.RemoveObj (_obj.id);
		
		_obj.gameObject.SetActive (false);
		_obj.id = -1;
		_obj.name = "Sound Obj (Close)";
		soundObjPoolList.Enqueue (_obj);
	}
#endregion

}
