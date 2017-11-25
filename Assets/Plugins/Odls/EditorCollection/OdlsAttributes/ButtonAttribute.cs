using UnityEngine;
using System.Collections;
using System.Reflection;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region "[Button]+函式名 = 無引數按鈕 | [Button(函式名)]+引數 = 有引數按鈕"
public class ButtonAttribute : PropertyAttribute  {	
	public string FunName;
	public ButtonAttribute(string p_name) {
		FunName = p_name; 
	}
	public ButtonAttribute() {
		FunName = ""; 
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (ButtonAttribute))]
public class ButtonDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_Position, SerializedProperty p_Property, GUIContent p_Label) {
		Rect _Position = p_Position;
		//EditorGUI.RectField (_Position,_Position);
		_Position.width -= 100;
		EditorGUI.PropertyField (_Position,p_Property);
		_Position.x += _Position.width;
		_Position.width = 100;
		ButtonAttribute _button = attribute as ButtonAttribute;

		if (_button.FunName == "") {
			if (p_Property.propertyType == SerializedPropertyType.String) {
				if (GUI.Button (_Position, p_Property.stringValue)) {
					Click (p_Property);
				}
			} else {
				EditorGUI.LabelField (p_Position, p_Label.text, "[Button] Only for String , you can use [Button(Funtion Name)]");
			}
		} else {
			if (GUI.Button (_Position, _button.FunName)) {
				Click (p_Property, _button.FunName);
			}
		}
	}
	public void Click(SerializedProperty p_Property){ 
		object _obj=p_Property.serializedObject.targetObject;
		MethodInfo _method = _obj.GetType().GetMethod(p_Property.stringValue);
		_method.Invoke(_obj,null);
	}
	public void Click(SerializedProperty p_Property,string p_FunName){ 
		object _obj=p_Property.serializedObject.targetObject;
		MethodInfo _method = _obj.GetType().GetMethod(p_FunName);
		object[] _arg=new object[1];

		switch (p_Property.propertyType) {
		case SerializedPropertyType.Integer:
			_arg[0] = p_Property.intValue;
			break;
		case SerializedPropertyType.String:
			_arg[0] = p_Property.stringValue;
			break;
		case SerializedPropertyType.Boolean:
			_arg[0] = p_Property.boolValue;
			break;
		case SerializedPropertyType.Float:
			_arg[0] = p_Property.floatValue;
			break;
		case SerializedPropertyType.Enum:
			_arg[0] = p_Property.enumValueIndex;
			break;
		case SerializedPropertyType.LayerMask:
			_arg[0] = p_Property.intValue;
			break;
		case SerializedPropertyType.ObjectReference:
			_arg[0] = p_Property.objectReferenceValue;
			break;
		case SerializedPropertyType.Color:
			_arg[0] = p_Property.colorValue;
			break;
		case SerializedPropertyType.AnimationCurve:
			_arg[0] = p_Property.animationCurveValue;
			break;
		case SerializedPropertyType.Rect:
			_arg[0] = p_Property.rectValue;
			break;
		case SerializedPropertyType.Bounds:
			_arg[0] = p_Property.boundsValue;
			break;		
		case SerializedPropertyType.Vector2:
			_arg[0] = p_Property.vector2Value;
			break;
		case SerializedPropertyType.Vector3:
			_arg[0] = p_Property.vector3Value;
			break;
		case SerializedPropertyType.Vector4:
			_arg[0] = p_Property.vector4Value;
			break;
		default:
			Debug.LogError("[Button] Not support this type");
			return;
		}
		_method.Invoke(_obj,_arg);
	}
}
#endif
#endregion

#region "[URL(文字標籤_可選,url)] 開啟連結"
public class URLAttribute : PropertyAttribute  {	
	public string lable;
	public string url;
	public URLAttribute(string p_url) {
		lable = "URL";
		url = p_url; 
	}
	public URLAttribute(string p_lable,string p_url) {
		lable = p_lable;
		url = p_url; 
	}  
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (URLAttribute))]
public class URLDrawer : DecoratorDrawer {
	public override void OnGUI (Rect p_Position) {
		URLAttribute _url = attribute as URLAttribute;

		Rect _Position = p_Position;
		_Position.width /= 2.0f; 
		GUI.Label (_Position,_url.lable);

		_Position.x += _Position.width;
		if (GUI.Button (_Position,_url.url)) {
			Application.OpenURL(_url.url);
		}
	}
}
#endif
#endregion

#region "[Project(文字標籤_可選,路徑)] 開啟專案下的檔案"
public class ProjectAttribute : PropertyAttribute  {	
	public string lable;
	public string path;
	public string name;
	public ProjectAttribute(string p_path) {
		lable = "Project File";
		path = p_path; 
		name = Path.GetFileName (path);
	}
	public ProjectAttribute(string p_lable,string p_path) {
		lable = p_lable;
		path = p_path; 
		name = Path.GetFileName (path);
	}  
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (ProjectAttribute))]
public class ProjectDrawer : DecoratorDrawer {
	public override void OnGUI (Rect p_Position) {
		ProjectAttribute _project = attribute as ProjectAttribute;
		
		Rect _Position = p_Position;
		_Position.width /= 2.0f; 
		GUI.Label (_Position,_project.lable);
		
		_Position.x += _Position.width;
		if (GUI.Button (_Position,_project.name)) {
			Application.OpenURL("file://"+Directory.GetParent(Application.dataPath)+"/"+_project.path);
		}
	}
}
#endif
#endregion

#region "[ObjJump]跳至指定物件"
public class ObjJumpAttribute : PropertyAttribute  {
	public ObjJumpAttribute() {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (ObjJumpAttribute))]
public class ObjJumpDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_Position, SerializedProperty p_Property, GUIContent p_Label) {
		if (p_Property.propertyType != SerializedPropertyType.ObjectReference) {
			EditorGUI.LabelField (p_Position, p_Label.text, "[ObjJump] Only for GameObject");
		}else if(!p_Property.objectReferenceValue) {
			Rect _Position = p_Position;
			EditorGUI.BeginProperty(_Position, p_Label,p_Property);
			EditorGUI.PropertyField (_Position,p_Property);
			EditorGUI.EndProperty();
		}else if(p_Property.objectReferenceValue.GetType()!=typeof(GameObject)){
			EditorGUI.LabelField (p_Position, p_Label.text, "[ObjJump] Only for GameObject(ReferenceValue not GameObject)");
		}else{
			Rect _Position = p_Position;
			EditorGUI.BeginProperty(_Position, p_Label,p_Property);
			_Position.width -= 100;
			EditorGUI.PropertyField (_Position,p_Property);
			_Position.x += _Position.width;
			_Position.width = 100;
//			ObjJumpAttribute _button = attribute as ObjJumpAttribute;
			if (GUI.Button (_Position,"Jump")) {
				GameObject _obj=(GameObject)p_Property.objectReferenceValue;
				if(_obj){
					Selection.activeGameObject = _obj;
				}
			}
			EditorGUI.EndProperty();
		} 

	}
}
#endif
#endregion