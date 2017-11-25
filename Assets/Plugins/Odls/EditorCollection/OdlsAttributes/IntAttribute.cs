using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

#region "[Flag] 顯示旗標"
public class FlagAttribute : PropertyAttribute {
	public string[] itemStrs;
	public string strName = "";
	public FlagAttribute (int p_len) {
		itemStrs = new string[p_len];
		int f;
		for (f=0; f<p_len; f++) {
			itemStrs[f] = f.ToString();
		}
	}
	public FlagAttribute (string p_strName) {
		strName = p_strName;
	}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (FlagAttribute))]
public class FlagDrawer : PropertyDrawer {
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
		return EditorGUI.GetPropertyHeight (property) * 2;
	}
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		if (property.propertyType == SerializedPropertyType.Integer) {
			position.height /= 2;

			FlagAttribute _Flag = attribute as FlagAttribute;

			EditorGUI.PropertyField(position,property, label);
			position.y += position.height;
			if(_Flag.itemStrs == null && _Flag.strName!=""){
				SerializedProperty _itemStrsProperty = property.serializedObject.FindProperty(_Flag.strName);
				if(_itemStrsProperty != null){
					_Flag.itemStrs = SerializedPropertyValue.GetArray<string>(_itemStrsProperty);
				}else{
					EditorGUI.LabelField (position, label.text, "Property : \"" + _Flag.strName + "\" Not Found");
					return;
				}
			}
			if(_Flag.itemStrs == null){
				EditorGUI.LabelField (position, label.text, "Property : \"" + _Flag.strName + "\" Not String Array");
			}else{
				property.intValue = ExtendGUI.Flag(position, property.intValue, _Flag.itemStrs);
			}
		} else {
			EditorGUI.LabelField (position, label.text, "[Flag] Only for Int");
		}
	}
}
#endif
#endregion
