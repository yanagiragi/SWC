#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtendGUILayout{
	public delegate T DrawObjectGate<T>(T p_obj);
	static T[] DrawArray<T>(T[] p_array,DrawObjectGate<T> p_draw,ref bool p_isExpanded){
		GUILayout.BeginVertical ();

		if (p_array == null) {
			p_array = new T[0];
		}
		int f;
		int len = p_array.Length;

		GUILayout.BeginHorizontal ();
		p_isExpanded = GUILayout.Toggle (p_isExpanded,"",GUILayout.Width(30));
		GUILayout.Label ("Size : " + len);
		GUILayout.EndHorizontal ();

		if (p_isExpanded) {
			if (len <= 0) {
				if (GUILayout.Button ("+")) {
					p_array = new T[1];
					len++;
				}
			} else {
				for (f=0; f<len; f++) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (f.ToString (), GUILayout.Width (30));

					p_array [f] = p_draw (p_array [f]);

					if (GUILayout.Button ("+", GUILayout.Width (20))) {
						List<T> _list = new List<T> (p_array);
						_list.Insert (f + 1, default(T));
						p_array = _list.ToArray ();
						len++;
					}
					if (GUILayout.Button ("-", GUILayout.Width (20))) {
						List<T> _list = new List<T> (p_array);
						_list.RemoveAt (f);
						p_array = _list.ToArray ();
						len--;
					}
					GUILayout.EndHorizontal ();
				}
			}
		}

		GUILayout.EndVertical ();

		return p_array;
	}

	static public T[] ObjectArray<T>(T[] p_array,ref bool p_isExpanded) where T : Object{
		return DrawArray<T> (p_array,DrawObject,ref p_isExpanded);
	}
	static T DrawObject<T>(T p_obj) where T : Object{
		p_obj = (T)EditorGUILayout.ObjectField(p_obj,typeof(T));
		return p_obj;
	}

	//Int
	static public int[] IntArray(int[] p_array,ref bool p_isExpanded){
		return DrawArray<int> (p_array,DrawInt,ref p_isExpanded);
	}
	static int DrawInt(int p_int){
		p_int = EditorGUILayout.IntField(p_int);
		return p_int;
	}

	//String
	static public string[] StringArray(string[] p_array,ref bool p_isExpanded){
		return DrawArray<string> (p_array,DrawString,ref p_isExpanded);
	}
	static string DrawString(string p_string){
		p_string = EditorGUILayout.TextField(p_string);
		return p_string;
	}

	//Bool
	static public bool[] BoolArray(bool[] p_array,ref bool p_isExpanded){
		return DrawArray<bool> (p_array,DrawBool,ref p_isExpanded);
	}
	static bool DrawBool(bool p_bool){
		p_bool = EditorGUILayout.Toggle(p_bool);
		return p_bool;
	}

	//Color
	static public Color[] ColorArray(Color[] p_array,ref bool p_isExpanded){
		return DrawArray<Color> (p_array,DrawColor,ref p_isExpanded);
	}
	static Color DrawColor(Color p_color){
		p_color = EditorGUILayout.ColorField(p_color);
		return p_color;
	}
}
#endif
