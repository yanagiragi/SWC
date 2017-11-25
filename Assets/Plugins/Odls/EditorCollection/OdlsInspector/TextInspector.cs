#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class TextInspector{
	static public void OnInspectorGUI(Text p_text){
		Font(p_text);
	}
	static void Font(Text p_text) {
		GUILayout.Label ("預設字體:"+UiCreatTool.fontPath);

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button ("使用預設字體")) {
			UiCreatTool.FixFont(p_text);
		}
		if (GUILayout.Button ("將字體設為預設值")) {
			UiCreatTool.SetFont(p_text.font);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button ("將字體套用到子物件")) {
			Text[] _texts = p_text.GetComponentsInChildren<Text>();
			SetFont(_texts,p_text.font);
		}
		if (GUILayout.Button ("將字體套用到Scene")) {
			Text[] _texts = GameObjectExtend.GetComponentsInScene<Text>();
			SetFont(_texts,p_text.font);
		}
		EditorGUILayout.EndHorizontal();
	}
	static void SetFont(Text[] p_texts,Font p_font) {
		int f;
		int len = p_texts.Length;
		for(f=0; f<len; f++){
			p_texts[f].font = p_font;
		}
	}
}
#endif