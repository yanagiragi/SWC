#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
public class UiTextPositioning : MonoBehaviour {
	public static CanvasScaler canvasScaler;
	List<Rect> uiRectList = new List<Rect>();
	List<Rect> worldRectList = new List<Rect>();
	Text m_text;
	public Text text {
		get{
			if(m_text == null){
				m_text = GetComponent<Text> ();
			}
			return m_text;
		}
	}
	void Start () {}
	void Update () {}

#region "Target"
	public List<Rect> GetTargetCharUiRectList(char p_char){
		return GetTargetCharRectList (p_char,uiRectList);
	}
	public List<Rect> GetTargetCharWorldRectList(char p_char){
		return GetTargetCharRectList (p_char,worldRectList);
	}
	List<Rect> GetTargetCharRectList(char p_char,List<Rect> p_rectList){
		List<Rect> _list = new List<Rect> ();
		int f;
		int len = Mathf.Min(text.text.Length,p_rectList.Count);
		char[] _chars = text.text.ToCharArray ();
		for (f=0; f<len; f++) {
			if(_chars[f] == p_char){
				_list.Add(p_rectList[f]);
			}
		}
		return _list;
	}

	[Button("GetTargetString")]public string GetTargetStringBut;
	public void GetTargetString(string p_string){
		GetTargetStringRectList (p_string,uiRectList);
	}

	public List<Rect> GetTargetStringUiRectList(string p_string){
		return GetTargetStringRectList (p_string,uiRectList);
	}
	public List<Rect> GetTargetStringWorldRectList(string p_string){
		return GetTargetStringRectList (p_string,worldRectList);
	}
	List<Rect> GetTargetStringRectList(string p_string,List<Rect> p_rectList){
		List<Rect> _list = new List<Rect> ();
		int f;
		
		string _text = text.text;
		int _textLen = Mathf.Min(_text.Length,p_rectList.Count);
		int _targetLen = p_string.Length;
		int _indexStart,_indexEnd;
		Rect _tempRect;
		if (_targetLen > 0) {
			Debug.Log("GetTargetString [" + p_string +"] in [" + _text + "]");
			for (f=0; f<(_textLen-_targetLen+1); f++) {
				_indexStart = _text.IndexOf (p_string, f);
				if (_indexStart >= 0) {

					_indexEnd = _indexStart + _targetLen -1;
					Debug.Log("from " + _indexStart + "to" + _indexEnd);

					_tempRect = p_rectList[_indexStart];
					for (f=_indexStart+1 ; f<_indexEnd + 1; f++) {
						_tempRect.xMax = Mathf.Max(_tempRect.xMax, p_rectList[f].xMax);
						_tempRect.yMax = Mathf.Max(_tempRect.yMax, p_rectList[f].yMax);
						_tempRect.xMin = Mathf.Min(_tempRect.xMin, p_rectList[f].xMin);
						_tempRect.yMin = Mathf.Min(_tempRect.yMin, p_rectList[f].yMin);
					}

					_list.Add(_tempRect);
				} else {
					break;
				}
			}
		}
		return _list;
	}
#endregion

#region "RectList"
	public List<Rect> GetUiRectList(){
		return uiRectList;
	}
	public List<Rect> GetWorldRectList(){
		return worldRectList;
	}

	[Button]public string getInfoBut = "RefreshRectList";
	public void RefreshRectList () {
		string _str = text.text;
//		string _log = "";

		TextGenerator _generator = text.cachedTextGenerator;

		IList<UIVertex> _vertList = _generator.verts;

		worldRectList.Clear();
		uiRectList.Clear ();

		if (canvasScaler == null) {
			canvasScaler = GameObjectExtend.GetComponentInScene<CanvasScaler> ();
			if (canvasScaler == null) {
				Debug.LogError("UiTextPositioning No CanvasScaler");
				return;
			}
		}

		RectTransform _rectTransform = canvasScaler.gameObject.GetComponent<RectTransform>();
		Vector2 _uiResolution = _rectTransform.sizeDelta;
		Vector2 _screenResolution = GameObjectExtend.GetViewSize();
		Vector2 _uiScale = new Vector2 (_uiResolution.x/_screenResolution.x,
		                                _uiResolution.y/_screenResolution.y);
		Vector2 _worldScale = (Vector2)transform.lossyScale;



		int f;
		int _vertLel = _vertList.Count / 4;
		int len = _str.Length;

		if (_vertLel != len) {
			len = Mathf.Min(len,_vertLel-1);
		}

		for (f=0; f<len; f++){
			Vector2 _rtPos = _vertList[f*4+1].position;
			Vector2 _lbPos = _vertList[f*4+3].position;

			_rtPos.x *= _uiScale.x;
			_rtPos.y *= _uiScale.y;
			_lbPos.x *= _uiScale.x;
			_lbPos.y *= _uiScale.y;

			Rect _uiRect = new Rect(_lbPos,_rtPos-_lbPos);
			uiRectList.Add(_uiRect);

			_rtPos.x *= _worldScale.x;
			_rtPos.y *= _worldScale.y;
			_lbPos.x *= _worldScale.x;
			_lbPos.y *= _worldScale.y;
			Rect _worldRect = new Rect(_lbPos,_rtPos-_lbPos);
			worldRectList.Add(_worldRect);
		}

	#if UNITY_EDITOR
		lastStr = _str;
		SceneView.RepaintAll();
	#endif
	}


#endregion

#if UNITY_EDITOR
	string lastStr = "";
	public void OnDrawGizmosSelected () {
		if (text.text != lastStr) {
			RefreshRectList();
		}
		
		if (worldRectList != null) {
			int f;
			int len = worldRectList.Count;
			for (f=0; f<len; f++){
				Gizmos.DrawWireCube(transform.position + (Vector3)worldRectList[f].center,(Vector3)worldRectList[f].size);
			}
		}
	}
#endif
}
