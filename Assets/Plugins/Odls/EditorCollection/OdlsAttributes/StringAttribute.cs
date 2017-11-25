using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region "[Upper] 轉為大寫"
public class UpperAttribute : PropertyAttribute {
	public UpperAttribute () {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (UpperAttribute))]
public class UpperDrawer : PropertyDrawer {
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		if (property.propertyType == SerializedPropertyType.String) {
			EditorGUI.PropertyField(position,property, label);
			property.stringValue=property.stringValue.ToUpper();
		} else {
			EditorGUI.LabelField (position, label.text, "[Upper] Only for String");
		}
	}
}
#endif
#endregion

#region "[Lower] 轉為小寫"
public class LowerAttribute : PropertyAttribute {
	public LowerAttribute () {}
}
#if UNITY_EDITOR
[CustomPropertyDrawer (typeof (LowerAttribute))]
public class LowerDrawer : PropertyDrawer {
	public override void OnGUI (Rect p_Position, SerializedProperty p_Property, GUIContent Label) {
		if (p_Property.propertyType == SerializedPropertyType.String) {
			EditorGUI.PropertyField(p_Position,p_Property, Label);
			p_Property.stringValue=p_Property.stringValue.ToLower();
		} else {
			EditorGUI.LabelField (p_Position, Label.text, "[Lower] Only for String");
		}
	}
}
#endif
#endregion


//public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//	return property.isExpanded ? 32f : 16f;
//}