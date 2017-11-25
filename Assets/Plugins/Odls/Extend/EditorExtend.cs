#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

public class EditorExtend {
	static public void RefreshProject(string p_assetPath){
		Object _asset = AssetDatabase.LoadMainAssetAtPath (p_assetPath);
		RefreshProject (_asset);
	}
	static public void RefreshProject(Object p_asset){
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = p_asset;
	}
	public static T CreateScriptable<T> (string p_path) where T : ScriptableObject{
		T _asset = ScriptableObject.CreateInstance<T> ();
		FileExtend.PrepareDirectory (Application.dataPath.Substring(0,Application.dataPath.Length - 7) + "/" + p_path);
		AssetDatabase.CreateAsset (_asset, p_path);
		RefreshProject (_asset);
		return _asset;
	}
}
#endif