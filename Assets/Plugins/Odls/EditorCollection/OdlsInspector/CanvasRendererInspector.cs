#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects()]
[CustomEditor(typeof(CanvasRenderer),true)]
public class CanvasRendererInspector : Editor  {
	enum E_CanvasRendererType{None,Text,Image,RawImage};
	E_CanvasRendererType type = E_CanvasRendererType.None;
	Component component;
	bool fold;
	string foldName;
	public void OnEnable(){

		CanvasRenderer _render = (CanvasRenderer)target;

		component = _render.GetComponent<Text>();
		if (component != null) {
			type = E_CanvasRendererType.Text;
			foldName = "Odls TextRenderer";
			fold = EditorPrefs.GetBool(foldName, false);
			return;
		}

		component = _render.GetComponent<Image>();
		if (component != null) {
			type = E_CanvasRendererType.Image;
			foldName = "Odls ImageRenderer";
			fold = EditorPrefs.GetBool(foldName, false);
			return;
		}

		component = _render.GetComponent<RawImage>();
		if (component != null) {
			type = E_CanvasRendererType.RawImage;
			foldName = "Odls RawImageRenderer";
			fold = EditorPrefs.GetBool(foldName, false);
			return;
		}

	}
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		if (type != E_CanvasRendererType.None) {
			bool _fold = EditorGUILayout.Foldout (fold, foldName);
			if (_fold != fold) {
				fold = _fold;
				EditorPrefs.SetBool (foldName, fold);
			}
			if (fold) {
				switch(type){
				case E_CanvasRendererType.Text:
					TextInspector.OnInspectorGUI((Text)component);
					break;
				case E_CanvasRendererType.Image:
					GUILayout.Label("Image 擴充\n咱還沒想到要寫什麼");
					break;
				case E_CanvasRendererType.RawImage:
					GUILayout.Label("RawImage 擴充\n咱還沒想到要寫什麼");
					break;
				}
			
			}
		}
	}

}
#endif
