//改自
//https://github.com/twsiyuan/unity-ReorderableListUtility

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using UnityEngine;

public static class ReorderableListUtility
{
	public static ReorderableList CreateAutoLayout(SerializedProperty property, float columnSpacing = 4f, System.Action<Rect, SerializedProperty> ExGUI = null)
	{
		return CreateAutoLayout(property, true, true, true, true, null, null, columnSpacing, ExGUI);
	}
	
	public static ReorderableList CreateAutoLayout(SerializedProperty property, string[] headers, float?[] columnWidth = null, float columnSpacing = 4f, System.Action<Rect, SerializedProperty> ExGUI = null)
	{
		return CreateAutoLayout(property, true, true, true, true, headers, columnWidth, columnSpacing, ExGUI);
	}
	
	public static ReorderableList CreateAutoLayout(SerializedProperty property, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton, float columnSpacing = 4f, System.Action<Rect, SerializedProperty> ExGUI = null)
	{
		return CreateAutoLayout(property, draggable, displayHeader, displayAddButton, displayRemoveButton, null, null, columnSpacing, ExGUI);
	}
	
	public static ReorderableList CreateAutoLayout(SerializedProperty property, bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton, string[] headers, float?[] columnWidth = null, float columnSpacing = 4f, System.Action<Rect, SerializedProperty> ExGUI = null)
	{
		ReorderableList list = new ReorderableList(property.serializedObject, property, draggable, displayHeader, displayAddButton, displayRemoveButton);
		List<Column> colmuns = new List<Column>();
		
		list.drawElementCallback = DrawElement(list, GetColumnsFunc(list, headers, columnWidth, colmuns), columnSpacing,ExGUI);
		list.drawHeaderCallback = DrawHeader(list, GetColumnsFunc(list, headers, columnWidth, colmuns), columnSpacing);

		return list;
	}
	
	public static bool DoLayoutListWithFoldout(ReorderableList list, string label = null)
	{
		SerializedProperty property = list.serializedProperty;
		property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label != null ? label : property.displayName);
		if (property.isExpanded)
		{
			list.DoLayoutList();
		}
		return property.isExpanded;
	}

	public static bool DoListWithFoldout(ReorderableList list, Rect rect, string label = null)
	{
		SerializedProperty property = list.serializedProperty;
		rect.height = 16;
		property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label != null ? label : property.displayName);
		if (property.isExpanded)
		{
			rect.y += 16;
			list.DoList(rect);
		}
		
		return property.isExpanded;
	}

	private static ReorderableList.ElementCallbackDelegate DrawElement(ReorderableList list, System.Func<List<Column>> getColumns, float columnSpacing,System.Action<Rect, SerializedProperty> ExGUI)
	{
		return (rect, index, isActive, isFocused) =>
		{
			SerializedProperty property = list.serializedProperty;
			SerializedProperty elementProperty = property.GetArrayElementAtIndex(index);
			List<Column> columns = getColumns();
			List<float> layouts = CalculateColumnLayout(columns, rect, columnSpacing);

			rect.height = EditorGUIUtility.singleLineHeight;

			Rect arect = new Rect(rect);

			if(elementProperty.propertyType == SerializedPropertyType.Generic){
				for (int ii = 0; ii < columns.Count; ii++)
				{
					Column c = columns[ii];
					
					arect.width = layouts[ii];
					SerializedProperty subProperty = elementProperty.FindPropertyRelative(c.PropertyName);
					if(subProperty.isArray && (subProperty.propertyType != SerializedPropertyType.String)){
						GUI.Label(arect,"  [" + subProperty.arraySize + "]");
					}else{
						EditorGUI.PropertyField(arect, subProperty , GUIContent.none);
					}
					arect.x += arect.width + columnSpacing;
				}
			}else{
				EditorGUI.PropertyField(arect, elementProperty , GUIContent.none);
			}
			rect.width += columnSpacing;
			if(ExGUI != null){
				ExGUI.Invoke(rect,elementProperty);
			}
		};
	}
	
	private static ReorderableList.HeaderCallbackDelegate DrawHeader(ReorderableList list, System.Func<List<Column>> getColumns, float columnSpacing)
	{
		return (rect) =>
		{
			List<Column> columns = getColumns();
			
			if (list.draggable)
			{
				rect.width -= 15;
				rect.x += 15;
			}

//			rect.width -= 120;

			List<float> layouts = CalculateColumnLayout(columns, rect, columnSpacing);
			Rect arect = rect;
			arect.height = EditorGUIUtility.singleLineHeight;
			for (int ii = 0; ii < columns.Count; ii++)
			{
				Column c = columns[ii];
				
				arect.width = layouts[ii];
				EditorGUI.LabelField(arect, c.DisplayName);
				arect.x += arect.width + columnSpacing;
			}
		};
	}
	
	private static System.Func<List<Column>> GetColumnsFunc(ReorderableList list, string[] headers, float?[] columnWidth, List<Column> output)
	{
		SerializedProperty property = list.serializedProperty;
		return () =>
		{
			if (output.Count <= 0 || list.serializedProperty != property)
			{
				output.Clear();
				property = list.serializedProperty;
				
				if (property.isArray && property.arraySize > 0)
				{
					SerializedProperty it = property.GetArrayElementAtIndex(0).Copy();
					string prefix = it.propertyPath;
					int index = 0;
					if (it.Next(true))
					{
						do
						{
							if (it.propertyPath.StartsWith(prefix))
							{
								Column c = new Column();
								c.DisplayName = (headers != null && headers.Length > index) ? headers[index] : it.displayName;
								c.PropertyName = it.propertyPath.Substring(prefix.Length + 1);
								c.Width = (columnWidth != null && columnWidth.Length > index) ? columnWidth[index] : null;
								
								output.Add(c);
							}
							else
							{
								break;
							}
							
							index += 1;
						}
						while (it.Next(false));
					}
				}
			}
			
			return output;
		};
	}
	
	private static List<float> CalculateColumnLayout(List<Column> columns, Rect rect, float columnSpacing)
	{
		float autoWidth = rect.width;
		int autoCount = 0;
		foreach (Column column in columns)
		{
			if (column.Width.HasValue)
			{
				autoWidth -= column.Width.Value;
			}
			else
			{
				autoCount += 1;
			}
		}
		
		autoWidth -= (columns.Count - 1) * columnSpacing;
		autoWidth /= autoCount;
		
		List<float> widths = new List<float>(columns.Count);
		foreach (Column column in columns)
		{
			if (column.Width.HasValue)
			{
				widths.Add(column.Width.Value);
			}
			else
			{
				widths.Add(autoWidth);
			}
		}
		
		return widths;
	}
	
	private struct Column
	{
		public string DisplayName;
		public string PropertyName;
		public float? Width;
	}
}
#endif