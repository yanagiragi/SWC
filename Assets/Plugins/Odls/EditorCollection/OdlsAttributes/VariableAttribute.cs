using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

#region "[LockInPlay] 在執行時禁止更動"
public class LockInPlayAttribute : PropertyAttribute {
	public LockInPlayAttribute () {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (LockInPlayAttribute))]
public class LockInPlayDrawer : PropertyDrawer {
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		return SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		if (Application.isPlaying)GUI.enabled=false;
		EditorGUI.PropertyField (p_position, p_property, p_Label, true);
		if (Application.isPlaying)GUI.enabled=true;
	}
}
#endif
#endregion

#region "[LockInEdit] 在編輯時禁止更動"
public class LockInEditAttribute : PropertyAttribute {
	public LockInEditAttribute () {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (LockInEditAttribute))]
public class LockInEditDrawer : PropertyDrawer {
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		return SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		if (!Application.isPlaying)GUI.enabled=false;
		EditorGUI.PropertyField (p_position, p_property, p_Label, true);
		GUI.enabled=true;
	}
}
#endif
#endregion

#region "[LockInInspector] 在面板中禁止更動"
public class LockInInspectorAttribute : PropertyAttribute {
	public LockInInspectorAttribute () {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (LockInInspectorAttribute))]
public class LockInInspectorDrawer : PropertyDrawer {
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		return SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		GUI.enabled=false;
		EditorGUI.PropertyField (p_position, p_property, p_Label, true);
		GUI.enabled=true;
	}
}
#endif
#endregion

#region "[NullAlarm(是否log_可選,是否暫停_可選)] 無物件警告"
public class NullAlarmAttribute : PropertyAttribute {
	public bool log=false;
	public bool pause=false;
	public bool isNull=false;
	public NullAlarmAttribute (bool p_log=false,bool p_pause=false) {
		log = p_log;
		pause = p_pause;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (NullAlarmAttribute))]
public class NullAlarmDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		if (p_property.propertyType == SerializedPropertyType.ObjectReference) {
			NullAlarmAttribute _Alarm = attribute as NullAlarmAttribute;
			if(p_property.objectReferenceValue==null){
				GUI.color=Color.red;
				if(!_Alarm.isNull){
					MonoBehaviour _Mono=(MonoBehaviour)p_property.serializedObject.targetObject;
					if(_Alarm.log){Debug.LogWarning("Attribute Alarm: " + _Mono.name + " / " + p_property.name + " = Null");}
					if(_Alarm.pause){Debug.Break();}
				}
			}
			_Alarm.isNull=(p_property.objectReferenceValue==null);
			EditorGUI.PropertyField(p_position,p_property, p_Label);
			GUI.color=Color.white;
		} else {
			EditorGUI.LabelField (p_position, p_Label.text, "[NullAlarm] Only for GameObject");
		}
	}
}
#endif
#endregion

#region "[EqualAlarm(閥值,是否log_可選,是否暫停_可選)] 等於值警告"
public class EqualAlarmAttribute : PropertyAttribute {
	public float floatTarget=-999999;
	public string strTarget="";
	public float floatNow=-999999;
	public string strNow="";
	public bool log=false;
	public bool pause=false;
	public EqualAlarmAttribute(float p_target,bool p_log=false,bool p_pause=false) {
		floatTarget = p_target;
		log = p_log;
		pause = p_pause;
	}
	public EqualAlarmAttribute(String p_target,bool p_log=false,bool p_pause=false) {
		strTarget = p_target;
		log = p_log;
		pause = p_pause;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (EqualAlarmAttribute))]
public class EqualAlarmDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		object _obj = SerializedPropertyValue.GetValue (p_property);
		EqualAlarmAttribute _Alarm = attribute as EqualAlarmAttribute;

		if (SerializedPropertyValue.isNumber(p_property)) {
			float _number;
			if(p_property.propertyType==SerializedPropertyType.Float){_number=(float)_obj;}
			else{_number=(int)_obj;}
			if(_number==_Alarm.floatTarget){
				GUI.color=Color.red;
				if(_Alarm.floatNow!=_number){
					MonoBehaviour _Mono=(MonoBehaviour)p_property.serializedObject.targetObject;
					if(_Alarm.log){Debug.LogWarning("Attribute Alarm: " + _Mono.name + " / " + p_property.name + " = " + _number + " , Equal: " + _Alarm.floatTarget);}
					if(_Alarm.pause){Debug.Break();}
				}
			}
			_Alarm.floatNow=_number;
			EditorGUI.PropertyField(p_position,p_property, p_Label);
			GUI.color=Color.white;
		}
		else if(p_property.propertyType==SerializedPropertyType.String){
			string _str=(string)_obj;
			if(_str==_Alarm.strTarget){
				GUI.color=Color.red;
				if(_Alarm.strNow!=_str){
					MonoBehaviour _Mono=(MonoBehaviour)p_property.serializedObject.targetObject;
					if(_Alarm.log){Debug.LogWarning("Attribute Alarm: " + _Mono.name + " / " + p_property.name + " = " + _str + " , Equal: " + _Alarm.strTarget);}
					if(_Alarm.pause){Debug.Break();}
				}
			}
			_Alarm.strNow=_str;
			EditorGUI.PropertyField(p_position,p_property, p_Label);
			GUI.color=Color.white;
		}
		else {
			EditorGUI.LabelField (p_position, p_Label.text, "[EqualAlarm] Only for Number or String");
		}
	}
}
#endif
#endregion

#region "[GreaterAlarm(閥值,是否log_可選,是否暫停_可選)] 大於值警告"
public class GreaterAlarmAttribute : PropertyAttribute {
	public float floatTarget=-999999;
	public float floatNow=-999999;
	public bool log=false;
	public bool pause=false;
	public GreaterAlarmAttribute(float p_target,bool p_log=false,bool p_pause=false) {
		floatTarget = p_target;
		log = p_log;
		pause = p_pause;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (GreaterAlarmAttribute))]
public class GreaterAlarmDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		object _obj = SerializedPropertyValue.GetValue (p_property);
		GreaterAlarmAttribute _Alarm = attribute as GreaterAlarmAttribute;
		
		if (SerializedPropertyValue.isNumber(p_property)) {
			float _number;
			if(p_property.propertyType==SerializedPropertyType.Float){_number=(float)_obj;}
			else{_number=(int)_obj;}
			if(_number>_Alarm.floatTarget){
				GUI.color=Color.red;
				if(_Alarm.floatNow!=_number){
					MonoBehaviour _Mono=(MonoBehaviour)p_property.serializedObject.targetObject;
					if(_Alarm.log){Debug.LogWarning("Attribute Alarm: " + _Mono.name + " / " + p_property.name + " = " + _number + " , Greater: " + _Alarm.floatTarget);}
					if(_Alarm.pause){Debug.Break();}
				}
			}
			_Alarm.floatNow=_number;
			EditorGUI.PropertyField(p_position,p_property, p_Label);
			GUI.color=Color.white;
		}
		else {
			EditorGUI.LabelField (p_position, p_Label.text, "[GreaterAlarm] Only for Number");
		}
	}
}
#endif
#endregion

#region "[LessAlarm(閥值,是否log_可選,是否暫停_可選)] 小於值警告"
public class LessAlarmAttribute : PropertyAttribute {
	public float floatTarget=-999999;
	public float floatNow=-999999;
	public bool log=false;
	public bool pause=false;
	public LessAlarmAttribute(float p_target,bool p_log=false,bool p_pause=false) {
		floatTarget = p_target;
		log = p_log;
		pause = p_pause;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (LessAlarmAttribute))]
public class LessAlarmDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		object _obj = SerializedPropertyValue.GetValue (p_property);
		LessAlarmAttribute _Alarm = attribute as LessAlarmAttribute;
		
		if (SerializedPropertyValue.isNumber(p_property)) {
			float _number;
			if(p_property.propertyType==SerializedPropertyType.Float){_number=(float)_obj;}
			else{_number=(int)_obj;}
			if(_number<_Alarm.floatTarget){
				GUI.color=Color.red;
				if(_Alarm.floatNow!=_number){
					MonoBehaviour _Mono=(MonoBehaviour)p_property.serializedObject.targetObject;
					if(_Alarm.log){Debug.LogWarning("Attribute Alarm: " + _Mono.name + " / " + p_property.name + " = " + _number + " , Less: " + _Alarm.floatTarget);}
					if(_Alarm.pause){Debug.Break();}
				}
			}
			_Alarm.floatNow=_number;
			EditorGUI.PropertyField(p_position,p_property, p_Label);
			GUI.color=Color.white;
		}
		else {
			EditorGUI.LabelField (p_position, p_Label.text, "[LessAlarm] Only for Number");
		}
	}
}
#endif
#endregion

#region "[Check(回傳bool的函式名)] = 值變化時檢查"
public class CheckAttribute : PropertyAttribute  {	
	public string FunName;
	public bool isFirst;
	public bool OK;
	public CheckAttribute(string p_name) {
		FunName = p_name;
		isFirst = true;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (CheckAttribute))]
public class CheckDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		CheckAttribute _checker = attribute as CheckAttribute;
		if (!_checker.OK) {GUI.color=Color.red;}

		EditorGUI.BeginChangeCheck ();
		Rect _Position = p_position;
		EditorGUI.PropertyField (_Position,p_property);

		if (EditorGUI.EndChangeCheck () || _checker.isFirst) {
			_checker.isFirst=false;
			_checker.OK=Check (p_property, _checker.FunName);
		}
	}
	public bool Check(SerializedProperty p_property,string p_FunName){ 
		object _obj=p_property.serializedObject.targetObject;
		MethodInfo _method = _obj.GetType().GetMethod(p_FunName);
		object[] _arg=new object[1];
		
		switch (p_property.propertyType) {
		case SerializedPropertyType.Integer:
			_arg[0] = p_property.intValue;
			break;
		case SerializedPropertyType.String:
			_arg[0] = p_property.stringValue;
			break;
		case SerializedPropertyType.Boolean:
			_arg[0] = p_property.boolValue;
			break;
		case SerializedPropertyType.Float:
			_arg[0] = p_property.floatValue;
			break;
		case SerializedPropertyType.Enum:
			_arg[0] = p_property.enumValueIndex;
			break;
		case SerializedPropertyType.LayerMask:
			_arg[0] = p_property.intValue;
			break;
		case SerializedPropertyType.ObjectReference:
			_arg[0] = p_property.objectReferenceValue;
			break;
		case SerializedPropertyType.Color:
			_arg[0] = p_property.colorValue;
			break;
		case SerializedPropertyType.AnimationCurve:
			_arg[0] = p_property.animationCurveValue;
			break;
		case SerializedPropertyType.Rect:
			_arg[0] = p_property.rectValue;
			break;
		case SerializedPropertyType.Bounds:
			_arg[0] = p_property.boundsValue;
			break;		
		case SerializedPropertyType.Vector2:
			_arg[0] = p_property.vector2Value;
			break;
		case SerializedPropertyType.Vector3:
			_arg[0] = p_property.vector3Value;
			break;
		case SerializedPropertyType.Vector4:
			_arg[0] = p_property.vector4Value;
			break;
		default:
			Debug.LogError("[Check] Not support this type");
			return false;
		}
		return (bool)_method.Invoke(_obj,_arg);
	}
}
#endif
#endregion

#region "[CopyPaste] = 複製貼上按鈕"
public class CopyPasteAttribute : PropertyAttribute  {
    #if UNITY_EDITOR
    public static SerializedProperty sourceSerializedProperty = null;
	public static object propertyObj = null;
	public static string propertyPath = "";

	public static bool isArray = false;
	public CopyPasteAttribute() {}
    #endif
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (CopyPasteAttribute))]
public class CopyPasteDrawer : PropertyDrawer {
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		string _path = p_property.propertyPath;
		bool _isFirst = _path.EndsWith(".data[0]");
		float _height = SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
		if(_isFirst){
			return _height + 18;			
		}else{
			return _height;
		}
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		string _path = p_property.propertyPath;
		bool _isFirst = _path.EndsWith(".data[0]");
		if(_isFirst){
			Rect _butRect = new Rect(p_position);
			int _indent = EditorGUI.indentLevel * 15;
			_butRect.height = 16;
			_butRect.x += _indent;
			_butRect.width -= _indent + 160;
			GUI.Label(_butRect,"        CopyPaste-Array");

			_butRect.x += _butRect.width;
			_butRect.width = 160;
			SerializedProperty _parentProperty = SerializedPropertyValue.GetParent(p_property);
			SerializedPropertyValue.DrawCopyPasteArray(_butRect,_parentProperty);
			p_position.y += 18;
		}

		CopyPasteAttribute _copyPaste = attribute as CopyPasteAttribute;

		Rect _copyPasteRect = new Rect(p_position);
		_copyPasteRect.x += p_position.width - 120;
		_copyPasteRect.width = 120;
		_copyPasteRect.height = 18;
		SerializedPropertyValue.DrawCopyPaste(_copyPasteRect,p_property);


		if((p_property.propertyType == SerializedPropertyType.Generic) || (p_property.isArray && (p_property.propertyType != SerializedPropertyType.String))){
			EditorGUI.PropertyField (p_position,p_property,true);
		}else{
			p_position.width -= 120;
			EditorGUI.PropertyField (p_position,p_property);
		}

	}

}
#endif
#endregion

#region "[ReorderableList] = 可排序列表"
public class ReorderableListAttribute : PropertyAttribute  {
	#if UNITY_EDITOR
	public ReorderableList list = null;
	#endif
	public ReorderableListAttribute() {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (ReorderableListAttribute))]
public class ReorderableListDrawer : PropertyDrawer {
	bool useOrder = false;
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		string _path = p_property.propertyPath;
		float _height = 0;
		ReorderableListAttribute _reorderableList = attribute as ReorderableListAttribute;
		bool _isFirst = _path.EndsWith(".data[0]");
		bool _isLast = false;

		if(_reorderableList.list != null){
			_isLast = _path.EndsWith(".data[" + (_reorderableList.list.count-1) + "]");
		}
		if(useOrder && (_reorderableList.list != null)){
			if(_isFirst){
				_height = _reorderableList.list.GetHeight() + 18;
			}else{
				_height = -2;
			}
		}else{
			_height = SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
		}

		if((_reorderableList.list != null) && !useOrder && _isLast){
			return _height + 18;
		}else{
			return _height;
		}
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_label) {
		string _path = p_property.propertyPath;
		if(!_path.EndsWith("]")){
			EditorGUI.LabelField (p_position, p_label.text, "[ReorderableList] Only for Array Or Collection");
			return;
		}

		ReorderableListAttribute _reorderableList = attribute as ReorderableListAttribute;
		bool _isFirst = _path.EndsWith(".data[0]");

		if(_reorderableList.list != null){			
			if(_isFirst){
				Rect _butRect = new Rect(p_position);
				int _indent = EditorGUI.indentLevel * 15;
				_butRect.height = 16;
				if(useOrder){
					_butRect.x += _indent +70;
					_butRect.width -= _indent + 70 + 160;
					if(GUI.Button(_butRect,"Finish Reorder")){
						useOrder = false;
					}
				}else{
					_butRect.x += _indent;
					_butRect.width -= _indent + 160;
					if(GUI.Button(_butRect,"Start Reorder")){
						useOrder = true;
					}
				}

				_butRect.x += _butRect.width;
				_butRect.width = 160;
				SerializedProperty _parentProperty = SerializedPropertyValue.GetParent(p_property);
				SerializedPropertyValue.DrawCopyPasteArray(_butRect,_parentProperty);
			}
		}

		SerializedObject _serializedObject = p_property.serializedObject;
		
		if(_isFirst){
			if(_reorderableList.list == null){
				SerializedProperty _parentProperty = SerializedPropertyValue.GetParent(p_property);
				_reorderableList.list = ReorderableListUtility.CreateAutoLayout(_parentProperty,4,SerializedPropertyValue.DrawCopyPasteBehind);				
			}
		}

		if(useOrder){
			if(_isFirst){
				p_position.width -= 120;
				ReorderableListUtility.DoListWithFoldout(_reorderableList.list,p_position);
			}
		}else{
			p_position.y += 16;

			Rect _copyPasteRect = new Rect(p_position);
			_copyPasteRect.x += p_position.width - 120;
			_copyPasteRect.width = 120;
			_copyPasteRect.height = 18;
			string _click = SerializedPropertyValue.DrawCopyPasteButton(_copyPasteRect,p_property);
			if(_click != ""){

			}else if((p_property.propertyType == SerializedPropertyType.Generic) || (p_property.isArray && (p_property.propertyType != SerializedPropertyType.String))){
				EditorGUI.PropertyField(p_position,p_property,p_label,true);
			}else{
				p_position.width -= 120;
				p_position.height = SerializedPropertyValue.GetPropertyHeight(p_property, p_label);
				EditorGUI.PropertyField (p_position,p_property);
			}
		}

	}
}
#endif
#endregion

#region "[V2Lable] 二軸顯示客製的標籤"
public class V2LableAttribute : PropertyAttribute {
	public string[] lables;
	public int[] lableWidths;
	public V2LableAttribute (string p_lableX,string p_lableY) {
		lables = new string[2];
		lables[0] = p_lableX;
		lables[1] = p_lableY;
		lableWidths = new int[2];
		lableWidths[0] = Mathf.Max(8 * lables[0].Length + 4, 12);
		lableWidths[1] = Mathf.Max(8 * lables[1].Length + 4, 12);
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (V2LableAttribute))]
public class V2LableDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		int _indent = EditorGUI.indentLevel;
		int _indentWidth = _indent * 15;
		
		if (p_property.propertyType == SerializedPropertyType.Vector2) {
			V2LableAttribute _border = attribute as V2LableAttribute;
			p_position.height = EditorGUIUtility.singleLineHeight;
			
			Vector2 _v2 = p_property.vector2Value;
			EditorGUI.LabelField(p_position,p_Label);
			
			EditorGUI.indentLevel = 0;
			
			p_position.width -= (EditorGUIUtility.labelWidth - _indentWidth);
			p_position.x += EditorGUIUtility.labelWidth;
			
			p_position.width /= 2;
			
			EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[0], p_position.width - 20);
			_v2.x = EditorGUI.FloatField(p_position,_border.lables[0],_v2.x);
			
			EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[1], p_position.width - 20);
			p_position.x += p_position.width;
			_v2.y = EditorGUI.FloatField(p_position,_border.lables[1],_v2.y);
						
			p_property.vector2Value = _v2;
			
			EditorGUI.indentLevel = _indent;
			EditorGUIUtility.labelWidth = 0;
			
		} else {
			EditorGUI.LabelField (p_position, p_Label.text, "[V2Lable] Only for Vector2");
		}
	}
}
#endif
#endregion

#region "[V3Lable] 三軸顯示客製的標籤"
public class V3LableAttribute : PropertyAttribute {
	public string[] lables;
	public int[] lableWidths;
	public V3LableAttribute (string p_lableX,string p_lableY,string p_lableZ) {
		lables = new string[3];
		lables[0] = p_lableX;
		lables[1] = p_lableY;
		lables[2] = p_lableZ;
		lableWidths = new int[3];
		lableWidths[0] = Mathf.Max(8 * lables[0].Length + 4, 12);
		lableWidths[1] = Mathf.Max(8 * lables[1].Length + 4, 12);
		lableWidths[2] = Mathf.Max(8 * lables[2].Length + 4, 12);
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (V3LableAttribute))]
public class V3LableDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		int _indent = EditorGUI.indentLevel;
		int _indentWidth = _indent * 15;
		
		if (p_property.propertyType == SerializedPropertyType.Vector3) {
			V3LableAttribute _border = attribute as V3LableAttribute;
			p_position.height = EditorGUIUtility.singleLineHeight;
			
			Vector3 _v3 = p_property.vector3Value;
			EditorGUI.LabelField(p_position,p_Label);
				
			EditorGUI.indentLevel = 0;

			p_position.width -= (EditorGUIUtility.labelWidth - _indentWidth);
			p_position.x += EditorGUIUtility.labelWidth;

			p_position.width /= 3;
			
			EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[0], p_position.width - 20);
			_v3.x = EditorGUI.FloatField(p_position,_border.lables[0],_v3.x);

			EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[1], p_position.width - 20);
			p_position.x += p_position.width;
			_v3.y = EditorGUI.FloatField(p_position,_border.lables[1],_v3.y);
			
			EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[2], p_position.width - 20);
			p_position.x += p_position.width;
			_v3.z = EditorGUI.FloatField(p_position,_border.lables[2],_v3.z);
			
			p_property.vector3Value = _v3;
			
			EditorGUI.indentLevel = _indent;
			EditorGUIUtility.labelWidth = 0;

		} else {
			EditorGUI.LabelField (p_position, p_Label.text, "[V3Lable] Only for Vector3");
		}
	}
}
#endif
#endregion

#region "[V4Lable] 四軸顯示客製的標籤"
public class V4LableAttribute : PropertyAttribute {
	public string[] lables;
	public int[] lableWidths;

	public V4LableAttribute (string p_lableX,string p_lableY,string p_lableZ,string p_lableW) {
		lables = new string[4];
		lables[0] = p_lableX;
		lables[1] = p_lableY;
		lables[2] = p_lableZ;
		lables[3] = p_lableW;
		lableWidths = new int[4];
		lableWidths[0] = Mathf.Max(8 * lables[0].Length + 4, 12);
		lableWidths[1] = Mathf.Max(8 * lables[1].Length + 4, 12);
		lableWidths[2] = Mathf.Max(8 * lables[2].Length + 4, 12);
		lableWidths[3] = Mathf.Max(8 * lables[3].Length + 4, 12);
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (V4LableAttribute))]
public class V4LableDrawer : PropertyDrawer {
	public override float GetPropertyHeight (SerializedProperty p_property, GUIContent p_label){
		if(p_property.isExpanded){
			return EditorGUIUtility.singleLineHeight * 3;
		}else{
			return EditorGUIUtility.singleLineHeight;
		}
	}
	public override void OnGUI (Rect p_position, SerializedProperty p_property, GUIContent p_Label) {
		int _indent = EditorGUI.indentLevel;
		int _indentWidth = _indent * 15;

		if (p_property.propertyType == SerializedPropertyType.Vector4) {
			V4LableAttribute _border = attribute as V4LableAttribute;
			p_position.height = EditorGUIUtility.singleLineHeight;

			Vector4 _v4 = p_property.vector4Value;

			float _width = p_position.width;
			p_position.width = EditorGUIUtility.labelWidth;
			p_property.isExpanded = EditorGUI.Foldout(p_position,p_property.isExpanded,p_Label);

			if(p_property.isExpanded){

				EditorGUI.indentLevel = 0;

				p_position.width = _width - (_indentWidth + 20);
				p_position.x += _indentWidth + 20;

				p_position.width /= 2;


				EditorGUIUtility.labelWidth = Mathf.Min(Math.Max(_border.lableWidths[0],_border.lableWidths[2]), p_position.width - 20);
				p_position.y += p_position.height;
				_v4.x = EditorGUI.FloatField(p_position,_border.lables[0],_v4.x);

				p_position.y += p_position.height;
				_v4.z = EditorGUI.FloatField(p_position,_border.lables[2],_v4.z);

				EditorGUIUtility.labelWidth = Mathf.Min(Math.Max(_border.lableWidths[1],_border.lableWidths[3]), p_position.width - 20);
				p_position.x += p_position.width;
				p_position.y -= p_position.height;
				_v4.y = EditorGUI.FloatField(p_position,_border.lables[1],_v4.y);

				p_position.y += p_position.height;
				_v4.w = EditorGUI.FloatField(p_position,_border.lables[3],_v4.w);

				p_property.vector4Value = _v4;

				EditorGUI.indentLevel = _indent;
				EditorGUIUtility.labelWidth = 0;
			}else{				
				EditorGUI.indentLevel = 0;
				
				p_position.width = _width - (EditorGUIUtility.labelWidth - _indentWidth);
				p_position.x += EditorGUIUtility.labelWidth;
				
				p_position.width /= 4;
				
				EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[0], p_position.width - 20);
				_v4.x = EditorGUI.FloatField(p_position,_border.lables[0],_v4.x);
				
				EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[1], p_position.width - 20);
				p_position.x += p_position.width;
				_v4.y = EditorGUI.FloatField(p_position,_border.lables[1],_v4.y);
				
				EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[2], p_position.width - 20);
				p_position.x += p_position.width;
				_v4.z = EditorGUI.FloatField(p_position,_border.lables[2],_v4.z);

				EditorGUIUtility.labelWidth = Mathf.Min(_border.lableWidths[3], p_position.width - 20);
				p_position.x += p_position.width;
				_v4.w = EditorGUI.FloatField(p_position,_border.lables[3],_v4.w);
			}

			p_property.vector4Value = _v4;
			
			EditorGUI.indentLevel = _indent;
			EditorGUIUtility.labelWidth = 0;
		} else {
			EditorGUI.LabelField (p_position, p_Label.text, "[V4Lable] Only for Vector4");
		}
	}
}
#endif
#endregion