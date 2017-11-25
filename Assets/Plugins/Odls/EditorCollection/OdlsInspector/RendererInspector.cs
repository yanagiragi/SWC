#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class RendererInspector : Editor  {
	string[] layerStrs;
	int[] layerIds;
	int len;
	int layerN;
	bool fold;
	public void OnEnable(){
		fold = EditorPrefs.GetBool("Odls Renderer fold", false);

		SortingLayer[] _layers=SortingLayer.layers;
		len = _layers.Length;
		layerStrs = new string[len+1];
		layerIds = new int[len];
		int f;
		string _NowName=((Renderer)target).sortingLayerName;
		for (f=0; f<len; f++) {
			layerStrs[f]=_layers[f].name;
			if(layerStrs[f]==_NowName){
				layerN=f;
			}
			layerIds[f]=_layers[f].id;
		}
		layerStrs [len] = "- Add Sortting Layer -";
	}

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		bool _fold = EditorGUILayout.Foldout (fold,"Odls Renderer");
		if (_fold!=fold) {
			fold=_fold;
			EditorPrefs.SetBool("Odls Renderer fold", fold);
		}
		if (fold) {
			Renderer _render = (Renderer)target;
			Layer(_render);
			Mate(_render);
		}
	}

	void Layer(Renderer _render) {
		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginChangeCheck();
		layerN = EditorGUILayout.Popup("Layer",layerN , layerStrs);
		if(EditorGUI.EndChangeCheck()) {
			if(layerN>=len){
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
			}
			else{
				foreach(Object _subRender in targets){ 
					Undo.RegisterUndo (_subRender, "Odls Render Layer");
					((Renderer)_subRender).sortingLayerID =layerIds[layerN];
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginChangeCheck();
		int order = EditorGUILayout.IntField("Sorting Order", _render.sortingOrder);
		if(EditorGUI.EndChangeCheck()) {
			foreach(Object _subRender in targets){ 
				Undo.RegisterUndo (_subRender, "Odls Render sortingOrder");
				((Renderer)_subRender).sortingOrder = order;
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	void Mate(Renderer _render) {
		if (GUILayout.Button("將材質球套用到子物件")) {
			Component[] _renders=_render.gameObject.GetComponentsInChildren(typeof(Renderer));
			foreach(Component _subRender in _renders){ 
				Undo.RegisterUndo (_subRender, "Odls Render Mate");
				((Renderer)_subRender).sharedMaterials=_render.sharedMaterials;
			}
		}
	}
}

[CanEditMultipleObjects()]
[CustomEditor(typeof(MeshRenderer),true)]
public class MeshRendererInspector : RendererInspector  {}
[CanEditMultipleObjects()]
[CustomEditor(typeof(SkinnedMeshRenderer),true)]
public class SkinnedMeshRendererInspector : RendererInspector  {}
[CanEditMultipleObjects()]
[CustomEditor(typeof(LineRenderer),true)]
public class LineRendererInspector : RendererInspector  {}
[CanEditMultipleObjects()]
[CustomEditor(typeof(TrailRenderer),true)]
public class TrailRendererInspector : RendererInspector  {}
[CanEditMultipleObjects()]
[CustomEditor(typeof(ParticleRenderer),true)]
public class ParticleRendererInspector : RendererInspector  {}
#endif
