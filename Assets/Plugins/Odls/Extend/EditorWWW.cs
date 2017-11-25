#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

//EditorWWW www1 = new EditorWWW("http://images.clipartpanda.com/numbers-clipart-0-number_0_purple.png",new string[]{"Start1","Clear","ReStart"});
//EditorWWW www2 = new EditorWWW("http://images.clipartpanda.com/numbers-clipart-0-number_0_purple.png",new string[]{"Start2","Clear","ReStart"});
//EditorWWW www3 = new EditorWWW("http://images.clipartpanda.com/numbers-clipart-0-number_0_purple.png",new string[]{"Start3","Clear","ReStart"});
//void wwwTest() {
//	EditorWWW.StartDoneCheck ();
//	if (GUILayout.Button ("Start All")) {
//		www1.StartWWW();
//		
//		www1.SetNext(www2);
//		www2.SetNext(www3);
//	}
//	www1.Run (true,true);
//	www2.Run (true,true);
//	www3.Run (true,true);
//	
//	if (!EditorWWW.EndDoneCheck ()) {
//		Repaint();
//	}
//}
public enum E_EditorWWW{none,run,done,error}
public class EditorWWW{
	static float loadingTime;
	static string[] loadingStr=new string[]{"╭(￣▁￣)╯","－(￣▃￣)－","╰(￣▄￣)╮","－(￣▃￣)－"};
	static string[] defaultButtonStr=new string[]{"Start","Clear","ReStart"};
	static bool DoneCheck;
	static public void StartDoneCheck(){
		DoneCheck = true;
	}
	static public bool EndDoneCheck(){
		if (!DoneCheck) {
			loadingTime=(loadingTime+0.03f)%4;
			GUILayout.Label ("Editor WWW Running... " + loadingStr [(int)Mathf.Floor (loadingTime)]);
		}
		return DoneCheck;
	}
	//---------------------------------
	string[] ButtonStr;
	public string url;
	public WWWForm form=null;
	public WWW www;
	EditorWWW nextWWW;
	E_EditorWWW state;
	public EditorWWW(string p_url,DoneGate p_OnSuccess,DoneGate p_OnFailed,string[] p_ButtonStr=null){
		Init(p_url,p_OnSuccess,p_OnFailed,p_ButtonStr);
	}
	public EditorWWW(string p_url,string[] p_ButtonStr=null){
		Init(p_url,null,null,p_ButtonStr);
	}
	void Init(string p_url,DoneGate p_OnSuccess=null,DoneGate p_OnFailed=null,string[] p_ButtonStr=null){
		url=p_url;
		OnSuccess = p_OnSuccess;
		OnFailed = p_OnFailed;
		if (p_ButtonStr == null) {
			ButtonStr = defaultButtonStr;
		} else if (p_ButtonStr.Length!=3) {
			ButtonStr = defaultButtonStr;
			Debug.LogError("EditorWWW ButtonStr Length Not 3:"+p_ButtonStr.ToString());
		} else {
			ButtonStr = p_ButtonStr;
		}
		state=E_EditorWWW.none;
	}
	public AssetBundle	assetBundle	{get{return www.assetBundle;}}
	public AudioClip	audioClip	{get{return www.GetAudioClip();}}
	public byte[] 		bytes		{get{return www.bytes;}}
	public string		error		{get{return www.error;}}
	public MovieTexture movie		{get{return www.GetMovieTexture();}}
	public string		text		{get{return www.text;}}
	public Texture2D	texture		{get{return www.texture;}}
	public bool			isDone		{get{return (state==E_EditorWWW.done);}}
	public delegate void DoneGate(WWW p_www);
	public DoneGate OnSuccess=null;
	public DoneGate OnFailed=null;
	public void SetNext(EditorWWW p_www,DoneGate p_OnSuccess=null,DoneGate p_OnFailed=null) {
		nextWWW = p_www;
		if(p_OnSuccess!=null){p_www.OnSuccess = p_OnSuccess;}
		if(p_OnFailed!=null){p_www.OnFailed = p_OnFailed;}
	}
	public void StartWWW(WWWForm p_form=null) {
		StartWWW (null,null,p_form);
	}
	public void StartWWW(DoneGate p_OnSuccess,DoneGate p_OnFailed = null,WWWForm p_form=null) {
		form = p_form;
		if (form!=null) {
			www = new WWW (url,form);
		} else {
			www = new WWW (url);
		}
		state=E_EditorWWW.run;
		if(p_OnSuccess!=null){OnSuccess = p_OnSuccess;}
		if(p_OnFailed!=null){OnFailed = p_OnFailed;}
		RunWWW();
	}
	void RunWWW() {
		if (Event.current.type == EventType.Repaint) {
			if (www.isDone) {
				//tex = www.texture;
				if (!string.IsNullOrEmpty (www.error)) {
					state = E_EditorWWW.error;
//					Debug.LogError("EditorWWW Error:"+www.error+" at:"+www.url);
					if (OnFailed != null) {
						OnFailed (www);
					}
				} else {
					state = E_EditorWWW.done;
					if (OnSuccess != null) {
						OnSuccess (www);
					}
					if (nextWWW != null) {
						nextWWW.StartWWW ();
					}
				}
			}
		}
		DoneCheck=false;
	}
	public E_EditorWWW Run(bool p_drawButton=false,bool p_drawProgress=false){
		switch (state) {
		case E_EditorWWW.none:
			if(p_drawButton){
				if (GUILayout.Button (ButtonStr[0])) { //"Start Button"
					StartWWW();
				}
			}
			break;
		case E_EditorWWW.run:
			if(p_drawProgress){
				GUILayout.Label ("Up: "+www.uploadProgress*100+"% Down: "+www.progress*100+"%");
			}
			RunWWW ();
			break;
		case E_EditorWWW.done:
			if(p_drawButton){
				if (GUILayout.Button (ButtonStr[1])) { //"Clear Button"
					www.Dispose();
					www=null;
					state=E_EditorWWW.none;
				}
			}
			break;
		case E_EditorWWW.error:
			if(p_drawButton){
				GUI.color=Color.red;
				if (GUILayout.Button (ButtonStr[2])) { //"ReStart Button"
					StartWWW();
				}
				GUI.color=Color.white;
			}
			break;
		}
		return state;
	}
}
#endif