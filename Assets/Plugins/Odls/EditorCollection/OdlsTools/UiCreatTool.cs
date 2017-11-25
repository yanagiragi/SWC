//http://www.xuanyusong.com/archives/4006
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class UiCreatTool{
	static public Font mainFont;
	static public string fontPath="";
	static public void SetFont(Font p_font){
		mainFont = p_font;
		string _path = AssetDatabase.GetAssetPath (mainFont);
		if(_path.Contains("Assets")){
			fontPath = _path;
			EditorPrefs.SetString ("UiCreatTool mainFont", fontPath);
			Debug.Log ("mainFont = "+fontPath);
		}else{
			EditorUtility.DisplayDialog("","預設值必須是 Assets 中的字體","╮(￣▽￣)╭");
		}
	}
	static Canvas getCanvas(){
		Canvas _canvas=null;
		if (Selection.activeTransform) {
			_canvas = Selection.activeTransform.GetComponentInParent<Canvas> ();
		}
		if (!_canvas) {
			GameObject _obj = new GameObject("Canvas",typeof(Canvas));
			_canvas=_obj.GetComponent<Canvas>();
			_canvas.renderMode=RenderMode.ScreenSpaceOverlay;


			_obj.AddComponent<CanvasScaler>();
			_obj.AddComponent<GraphicRaycaster>();
			_obj.gameObject.layer=LayerMask.NameToLayer("UI");


			GameObject _eventObj = new GameObject("EventSystem",typeof(EventSystem));

			_eventObj.AddComponent<StandaloneInputModule>();
//			_eventObj.AddComponent<TouchInputModule>();
			Selection.activeObject=_obj;

			Undo.RegisterCreatedObjectUndo (_obj, "Create UI");
			Undo.RegisterCreatedObjectUndo (_eventObj, "Create UI");
		}
		return _canvas;
	}

	static void FixObj(GameObject p_obj){
		p_obj.transform.SetParent(Selection.activeTransform);
		p_obj.transform.localPosition=Vector3.zero;
		p_obj.transform.localScale = Vector3.one;
		Selection.activeObject=p_obj;
		Undo.RegisterCreatedObjectUndo (p_obj, "Create UI");
	}
	
	[MenuItem("GameObject/UI/Image")]
	static Image CreatImage(){
		getCanvas();
		GameObject _obj = new GameObject("Image",typeof(Image));
		Image _image = _obj.GetComponent<Image> ();
		_image.raycastTarget = false;
		FixObj(_obj);
		return _image;
	}
	[MenuItem("GameObject/UI/Raw Image")]
	static RawImage CreatRawImage(){
		getCanvas();
		GameObject _obj = new GameObject("RawImage",typeof(RawImage));
		RawImage _rawImage = _obj.GetComponent<RawImage> ();
		_rawImage.raycastTarget = false;
		FixObj(_obj);
		return _rawImage;
	}
	[MenuItem("GameObject/UI/Text")]
	static Text CreatText(){
		getCanvas();
		GameObject _obj = new GameObject("Text",typeof(Text));
		Text _text=_obj.GetComponent<Text>();
		_text.raycastTarget = false;
		_text.rectTransform.sizeDelta = new Vector2(160,30);
		_text.text="New Text";
		FixFont(_text);
		FixObj(_obj);
		return _text;
	}
	[MenuItem("GameObject/UI/Button")]
	static Button CreatButton(){
		getCanvas();
		GameObject _obj = new GameObject("Button",typeof(Button));
		FixObj(_obj);
		Button _but=_obj.GetComponent<Button>();
		Image _image=_obj.AddComponent<Image>();
		_image.rectTransform.sizeDelta = new Vector2(160,30);
		Text _text = CreatText ();
		_text.transform.SetParent (_but.transform);
		_text.text = "Button";
		_text.color = Color.black;
		_text.alignment = TextAnchor.MiddleCenter;
		_text.rectTransform.sizeDelta = new Vector2 (0,0);
		_text.rectTransform.anchorMin = Vector2.zero;
		_text.rectTransform.anchorMax = Vector2.one;
		return _but;
	}
	static public void FixFont(Text p_text){
		fontPath = EditorPrefs.GetString ("UiCreatTool mainFont", "");
		if(!mainFont){
			mainFont=(Font)AssetDatabase.LoadMainAssetAtPath(fontPath);
		}
		if(!mainFont){
			Debug.Log("No Font:"+fontPath);
		}
		else{
			p_text.font=mainFont;
		}
	}

	[MenuItem("GameObject/UI/Panel")]
	static Image CreatPanel(){
		Canvas _canvas=getCanvas();
		GameObject _obj = new GameObject("Panel",typeof(Image));
		Image _image=_obj.GetComponent<Image>();
		_image.raycastTarget = false;
		_image.rectTransform.sizeDelta =_canvas.GetComponent<RectTransform>().sizeDelta;
		_image.color=new Color(255,255,255,100.0f/255f);
		FixObj(_obj);
		return _image;
	}
}
#endif