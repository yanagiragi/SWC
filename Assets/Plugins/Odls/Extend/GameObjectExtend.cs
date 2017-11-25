using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectExtend{
#region "Hierarchy"
	public static string GetFullPath(GameObject p_obj) {
		List<string> _pathList = new List<string>();
		
		Transform _nowTrans = p_obj.transform;
		_pathList.Add(_nowTrans.name);
		
		while (_nowTrans.parent != null) {
			_pathList.Insert(0, _nowTrans.parent.name);
			_nowTrans = _nowTrans.parent;
		}
		
		return string.Join("/", _pathList.ToArray());
	}
#endregion

#region "Transform"
	public static void SetLossyScale (Transform p_tran, Vector3 p_lossyScale) {
		Transform _parentTrans = p_tran.parent;
		if(_parentTrans == null){
			p_tran.localScale = p_lossyScale;
		}else{
			Vector3 _parentScale = _parentTrans.lossyScale;
			p_tran.localScale = new Vector3((_parentScale.x == 0 ? 0 : p_lossyScale.x / _parentScale.x),
			                                (_parentScale.y == 0 ? 0 : p_lossyScale.y / _parentScale.y),
			                                (_parentScale.z == 0 ? 0 : p_lossyScale.z / _parentScale.z));
		}
	}
#endregion

#region "Child"
	public static void DelAllChild(GameObject p_obj) {
		Transform[] _childs = p_obj.transform.GetComponentsInChildren<Transform>();
		int f;
		int len = _childs.Length;
		for (f=1; f<len; f++) {
			GameObject.Destroy(_childs[f].gameObject);
		}
	}
	public static void AddChild(GameObject p_obj, GameObject p_child){
		p_child.transform.SetParent (p_obj.transform);
	}
	public static void AddChildAndResetTransform(GameObject p_obj, GameObject p_child){
		AddChild(p_obj,p_child,Vector3.zero,Quaternion.identity,Vector3.one);
	}
	public static void AddChild(GameObject p_obj, GameObject p_child, Vector3 p_pos, Vector3 p_rota, Vector3 p_scale) {
		AddChild (p_obj, p_child, p_pos, Quaternion.Euler(p_rota), p_scale);
	}
	public static void AddChild(GameObject p_obj, GameObject p_child, Vector3 p_pos, Quaternion p_rota, Vector3 p_scale) {
		p_child.transform.SetParent (p_obj.transform);
		if (p_pos != null) {
			p_child.transform.localPosition = p_pos;
		}
		if (p_rota != null) {
			p_child.transform.localRotation = p_rota;
		}
		if (p_scale != null) {
			p_child.transform.localScale = p_scale;
		}
	}
#endregion

#region "Component"
	public static T  GetComponentInScene<T>() where T : Component{
		GameObject[] _obj = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().GetRootGameObjects ();
		T[] _components;
		
		int f,len2;
		int len = _obj.Length;
		for(f=0; f<len; f++){
			_components = _obj[f].transform.GetComponentsInChildren<T>(true);
			len2 = _components.Length;

			if(len2 <= 0){
				continue;
			}else{
				return _components[0];
			}
		}
		return null;
	}
	public static T[] GetComponentsInScene<T>() where T : Component{
		GameObject[] _obj = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().GetRootGameObjects ();
		T[] _components;
		List<T> _list = new List<T> ();
		
		int f,f2,len2;
		int len = _obj.Length;
		for(f=0; f<len; f++){
			_components = _obj[f].transform.GetComponentsInChildren<T>(true);
			len2 = _components.Length;
			for(f2=0; f2<len2; f2++){
				_list.Add(_components[f2]);
			}
		}
		
		return _list.ToArray ();
	}
#endregion

#region "Shader"
	static Dictionary<string,Shader> shaderDict = new Dictionary<string, Shader> ();
	public static void ReloadShader(GameObject p_obj){
		string _log = "";

		if(p_obj == null){
			Debug.LogError("ReloadShader Failed : No GameObject");
			return;
		}

		try{
			Renderer[] _renders = p_obj.GetComponentsInChildren<Renderer> (true);
			Material[] _materials;
			int f,f2;
			int len = _renders.Length;

			if (len <= 0) {
				Debug.LogError("No Material In GameObject : " + p_obj.name);
				return;
			}

			int len2;
			Shader _shader;
			_log += "Reload Shader : " + p_obj.name;
			for (f=0; f<len; f++) {
				_materials = _renders[f].sharedMaterials;

				len2 = _materials.Length;
				for (f2=0; f2<len2; f2++) {
					if(_materials[f2].shader == null){
						_log += "\n  Set Shader " + _renders[f].gameObject.name + " material " + f2.ToString() +
							StringExtend.RichColor(" Failed",Color.green) + "\n     Shader is Null";
					}else{
						string _name = _materials[f2].shader.name;

						_log += "\n  Set Shader [" + _name + "] to " + _renders[f].gameObject.name + " material " + f2.ToString();

						if(!shaderDict.TryGetValue(_name, out _shader)){
							_shader = Shader.Find(_name);
							if(_shader == null){
								_log += StringExtend.RichColor(" Failed",Color.green) + "\n     Can't Find Shader : [" + _name + "]";
								continue;
							}
						}

						_materials[f2].shader = _shader;
						_log += StringExtend.RichColor(" OK",Color.green);
					}
				}
			}
			Debug.Log (_log);
		}catch(System.Exception e){
			_log += StringExtend.RichColor("\nException : " + e.Message,Color.red);
			Debug.LogError (_log);
		}
	}
#endregion

#region "Editor"

	public static Vector2 GetViewSize(){
#if UNITY_EDITOR
		System.Type _typeofGameView = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo _GetSizeMethod = _typeofGameView.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object _result = _GetSizeMethod.Invoke(null,null);
		return (Vector2)_result;
#else
		return new Vector2(Screen.width,Screen.height);
#endif
	}
#endregion
}
