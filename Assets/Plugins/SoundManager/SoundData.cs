#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class SoundData{
	public SoundTable table;
	public string key = "newSound";
	public AudioClip clip = null;
	public float defaultVolume = 1;
	public bool defaultLoop = false;

	Dictionary<int,SoundPlayObj> soundObjDict = null;

	public SoundData(SoundTable p_table){
		table = p_table;
	}
	public SoundData(SoundData p_data){
		table = p_data.table;
		key = p_data.key;
		clip = p_data.clip;
		defaultVolume = p_data.defaultVolume;
		defaultLoop = p_data.defaultLoop;
	}

	public void Init(SoundTable p_table){
		table = p_table;
		soundObjDict = new Dictionary<int,SoundPlayObj> ();
	}

	public SoundPlayObj Play(float p_volume,bool p_loop){
		SoundPlayObj _obj = SoundManager.GetSoundPlayObj ();
		soundObjDict.Add (_obj.id,_obj);
		_obj.Play (this,p_volume,p_loop);
		return _obj;
	}
	List<SoundPlayObj> GetObjList(){
		List<SoundPlayObj> _objList = new List<SoundPlayObj> ();
		List<int> _nullIdList = new List<int> ();

		Dictionary<int,SoundPlayObj>.Enumerator _enumerator = soundObjDict.GetEnumerator();
		while (_enumerator.MoveNext()){
			KeyValuePair<int,SoundPlayObj> _pair = _enumerator.Current;
			if(_pair.Value == null){
				_nullIdList.Add(_pair.Key);
			}else{
				_objList.Add(_pair.Value);
			}
		}

		foreach (int _id in _nullIdList) {
			soundObjDict.Remove(_id);
		}

		return _objList;
	}
	public void Stop(){
		List<SoundPlayObj> _objList = GetObjList();
		foreach (SoundPlayObj _obj in _objList) {
			SoundManager.StopSoundPlayObj(_obj);
		}
	}
	public void RemoveObj(int p_id){
		soundObjDict.Remove (p_id);
	}
	public void RefreshVolume(){
		List<SoundPlayObj> _objList = GetObjList();
		foreach (SoundPlayObj _obj in _objList) {
			if(table.config.mute){
				_obj.SetVolume(_obj.volume,0);
			}else{
				_obj.SetVolume(_obj.volume,table.config.adjustVolume);
			}
		}
	}
#if UNITY_EDITOR
	public string OnInspectorGUI(SoundTableEditor p_editor,int p_index) {	
		string _action = "";

		switch (p_editor.tool) {
		case E_SOUND_TOOL.ITEM:
			DrawItem(p_editor,p_index,out _action);
			break;
		case E_SOUND_TOOL.DATA:
			DrawData(p_editor,p_index,out _action);
			break;
		case E_SOUND_TOOL.WATCH:
			DrawWatch(p_editor,p_index,out _action);
			break;
		}

		return _action;
	}

	void DrawItem(SoundTableEditor p_editor,int p_index,out string p_action){
		GUILayout.BeginHorizontal ();

		p_action = "";
		GUILayout.Label ("key",GUILayout.Width(30));
		
		EditorGUI.BeginChangeCheck();
		key = EditorGUILayout.TextField(key,GUILayout.Width(150));
		if(EditorGUI.EndChangeCheck()){
			p_editor.table.Init();
		}
		
		if(clip == null){
			GUILayout.Label ("clip[ null ]");
		}else{
			GUILayout.Label ("clip[ " + clip.name + " ]");
		}
		if(GUILayout.Button("Del",GUILayout.Width(40))){
			p_editor.table.DelSoundDataAt(p_index);
			p_action = "Del";
		}
		if(GUILayout.Button("Duplicate",GUILayout.Width(80))){
			SoundData _data = new SoundData(this);
			_data.key += "(Duplicate)";
			p_editor.table.InsertSoundDataAt(p_index + 1,_data);
			p_action = "Duplicate";
		}

		GUILayout.EndHorizontal ();
	}

	void DrawData(SoundTableEditor p_editor,int p_index,out string p_action){
		GUILayout.BeginHorizontal ();

		p_action = "";
		GUILayout.Label ("clip",GUILayout.Height(18));
		clip = (AudioClip)EditorGUILayout.ObjectField (clip,typeof(AudioClip));
		GUILayout.Label ("Loop");
		defaultLoop = GUILayout.Toggle (defaultLoop,"");
		GUILayout.Label ("Volume");
		defaultVolume = EditorGUILayout.Slider (defaultVolume,0f,1f,GUILayout.Width(150));

		GUILayout.EndHorizontal ();
	}

	void DrawWatch(SoundTableEditor p_editor,int p_index,out string p_action){
		GUILayout.BeginHorizontal ();

		p_action = "";
		GUILayout.Label ("key : " + key,GUILayout.Width(180));

		if(GUILayout.Button("Play Once",GUILayout.Width(90))){
			SoundManager.Play(table.name,key,false);
		}
		if(GUILayout.Button("Play Loop",GUILayout.Width(90))){
			SoundManager.Play(table.name,key,true);
		}
		if(GUILayout.Button("Stop All",GUILayout.Width(90))){
			SoundManager.Stop(table.name,key);
		}

		GUILayout.EndHorizontal ();

		if (soundObjDict != null) {
			foreach (SoundPlayObj _obj in soundObjDict.Values) {
				GUILayout.BeginHorizontal ();

				GUILayout.Label ("      Obj : " + _obj.id);
				if(GUILayout.Button("Close",GUILayout.Width(40))){
					_obj.Stop();
				}
				GUILayout.EndHorizontal ();
			}
		}
	}
#endif
}
