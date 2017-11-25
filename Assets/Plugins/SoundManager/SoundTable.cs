#if UNITY_EDITOR
using UnityEditor;
using System;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName="NewSoundTable",menuName="Sound Table",order=1)]
public class SoundTable : ScriptableObject {
	bool mIsLoaded = false;
	public SoundConfigTable config = null;
	public List<SoundData> soundList = new List<SoundData>();
	Dictionary<string,SoundData> soundDict = new Dictionary<string,SoundData>();

	public bool isLoaded{
		get{
			return mIsLoaded;
		}
	}

	public void Init(){
		if (Application.isPlaying) {
			mIsLoaded = true;
		}
		soundDict.Clear ();
		int f;
		int len = soundList.Count;
		for(f=0; f<len; f++){
			soundDict.Add(soundList[f].key,soundList[f]);
			soundList[f].Init(this);
		}
	}
	public void AddSoundData(SoundData p_data){
		if (soundDict.ContainsKey (p_data.key)) {
			Debug.Log(p_data.key + " Already Add, rename to :" + p_data.key + "'");
			p_data.key += "'";
			AddSoundData(p_data);
		} else {
			soundList.Add(p_data);
			soundDict.Add(p_data.key,p_data);
		}
	}
	public void DelSoundDataAt(int p_index){
		SoundData _data = soundList[p_index];
		soundList.RemoveAt (p_index);
		soundDict.Remove (_data.key);
	}
	public void InsertSoundDataAt(int p_index,SoundData p_data){
		if (soundDict.ContainsKey (p_data.key)) {
			Debug.LogError(p_data.key + " Already Add, rename to :" + p_data.key + "'");
			p_data.key += "'";
			InsertSoundDataAt(p_index,p_data);
		} else {
			soundList.Insert(p_index,p_data);
			soundDict.Add(p_data.key,p_data);
		}
	}
	public SoundData GetSoundData(string p_key){
		SoundData _soundData = null;
		soundDict.TryGetValue (p_key, out _soundData);
		return _soundData;
	}

	public void RefreshVolume(){
		int f;
		int len = soundList.Count;
		for(f=0; f<len; f++){
			soundList[f].RefreshVolume();
		}
	}
}

#if UNITY_EDITOR

public enum E_SOUND_TOOL{CONFIG,ITEM,DATA,WATCH};

[CustomEditor(typeof(SoundTable),true)]
public class SoundTableEditor : Editor {
	public E_SOUND_TOOL tool = E_SOUND_TOOL.CONFIG;
	public SoundTable table;
	Vector2 scroll = Vector2.zero;
	public void OnEnable(){
		if (target) {
			table = (SoundTable)target;
			if(!Application.isPlaying){
				table.Init();
			}
		}
	}
	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();

		if (table.isLoaded) {
			GUILayout.Label("isLoaded");
		}else{
			if(GUILayout.Button("Load")){
				SoundManager.GetTable(table.name);
			}
		}

		DrawTool ();

		switch (tool) {
		case E_SOUND_TOOL.CONFIG:
			DrawConfig ();
			break;
		case E_SOUND_TOOL.ITEM:
			DrawItem();
			break;
		case E_SOUND_TOOL.DATA:
			DrawData();
			break;
		case E_SOUND_TOOL.WATCH:
			DrawWatch ();
			break;
		}

		if(EditorGUI.EndChangeCheck()){
			EditorUtility.SetDirty(table);
		}
	}
	string[] toolStrs = new string[]{"Config","Item","Data","Watch"};
	void DrawTool() {
		tool = (E_SOUND_TOOL)GUILayout.Toolbar((int)tool,toolStrs);
	}

	#region "AddSelect"
	static int CompareAudioClipName(AudioClip p_A, AudioClip p_B){
//		int _intA = -1;
//		if (!int.TryParse (p_A.name.Split ("_" [0]) [1],out _intA)) {
//			_intA = int.MinValue;
//		}
//		int _intB = -1;
//		if (!int.TryParse (p_B.name.Split ("_" [0]) [1],out _intB)) {
//			_intB = int.MinValue;
//		}
//		
//		return _intA.CompareTo (_intB);

		return p_A.name.CompareTo (p_B.name);
	}
	void AddSelect() {
		UnityEngine.Object[] _objs = Selection.objects;
		int f;
		int len = _objs.Length;
		
		List<AudioClip> _clipList = new List<AudioClip>();
		for (f=0; f<len; f++) {
			if(_objs[f].GetType() == typeof(AudioClip)){
				_clipList.Add((AudioClip)_objs[f]);
			}
		}
		Comparison<AudioClip> _comparison = new Comparison<AudioClip>(CompareAudioClipName);
		_clipList.Sort (_comparison);
		
		len = _clipList.Count;
		
		for (f=0; f<len; f++) {
			SoundData _data = new SoundData(table);
			_data.clip = _clipList[f];
			_data.key = _clipList[f].name;
			table.AddSoundData(_data);
		}
	}
	#endregion

	#region"Config"
	void DrawConfig() {
		if (table.config == null) {
			GUI.color = Color.red;
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label ("Config");
		table.config = (SoundConfigTable)EditorGUILayout.ObjectField (table.config,typeof(SoundConfigTable));
		GUILayout.EndHorizontal ();

		if (table.config == null) {
			GUILayout.Label("      No Config");
		} else {
			table.config.OnInspectorGUI(this);
		}

		GUI.color = Color.white;
	}
	#endregion

	#region"Watch"
	void DrawWatch() {
		if (!Application.isPlaying) {
			GUILayout.Label("Watch Only Work when Playing");
			return;
		}

		DrawSoundList ();
		Repaint ();
	}
	#endregion

	#region"Item"
	void DrawItem() {
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add Select")) {
			AddSelect();
		}
		if (GUILayout.Button ("Add New")) {
			table.AddSoundData(new SoundData(table));
		}
		GUILayout.EndHorizontal ();

		DrawSoundList ();		
	}
	#endregion

	#region"Data"
	void DrawData() {		
		DrawSoundList ();		
	}
	#endregion

	#region"Sound List"
	void DrawSoundList() {
		scroll = GUILayout.BeginScrollView (scroll);

		int f;
		int len = table.soundList.Count;
		GUILayout.Label ("soundList :" + len);
		for (f=0; f<len; f++) {
			string _action = table.soundList[f].OnInspectorGUI(this,f);
			if(_action == "Del"){
				GUILayout.EndScrollView ();
				return;
			}
		}

		GUILayout.EndScrollView ();
	}
	#endregion
}
#endif