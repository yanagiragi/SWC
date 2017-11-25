#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor {
	enum E_TranItem{pos,rota,scale};
	static string[] lables = new string[]{"Position","Rotation","Scale"};
	enum E_SnapItem{posX,posY,posZ,rota,scale};

	public bool showTools;
	public bool copyPosition;
	public bool copyRotation;
	public bool copyScale;
	public bool pastePosition;
	public bool pasteRotation;
	public bool pasteScale;
	public bool selectionNullError;
	Transform tran;
	Vector3 pos;	Vector3 rota;	Vector3 scale;
	float[] Snaps=new float[5];
	Vector3 gridPos;
	bool gridWorld;

	bool fold;
	public override void OnInspectorGUI() {		
		tran = (Transform)target;

		EditorGUIUtility.LookLikeInspector();
		EditorGUI.indentLevel = 0;
		EditorGUI.BeginChangeCheck ();
		pos = DrawItem(tran.localPosition,E_TranItem.pos);
		rota = DrawItem(tran.localEulerAngles,E_TranItem.rota);
		scale = DrawItem(tran.localScale,E_TranItem.scale);
		bool _change = EditorGUI.EndChangeCheck ();

		bool _fold = EditorGUILayout.Foldout (fold,"Odls Transform");
		if (_fold!=fold) {
			fold=_fold;
			EditorPrefs.SetBool("Odls Transform fold", fold);
		}
		if (fold) {
			Child();
			Snap();
			Grid();
		}

		if (_change) {
			Undo.RegisterUndo (tran, "Odls Transform Change");
			tran.localPosition = pos;
			tran.localEulerAngles = rota;
			tran.localScale =scale;
		}
	}
	bool childFold;
	void Child() {
		EditorGUI.indentLevel = 1;
		GameObject _target;

		bool _childFold = EditorGUILayout.Foldout (childFold,"設定子物件");
		if (_childFold!=childFold) {
			childFold=_childFold;
			EditorPrefs.SetBool("Odls Transform childFold", childFold);
		}

		if(childFold){
			EditorGUI.indentLevel = 2;

			GUILayout.BeginHorizontal();
			GUILayout.Label ("        \\");
			GUILayout.Label ("子物件");
			GUILayout.Label ("父物件");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label ("        普通");
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			if(_target){
				Undo.RegisterUndo (_target.transform, "Odls Transform child");
				_target.transform.parent=tran;
			}
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			if(_target){
				Undo.RegisterUndo (tran, "Odls Transform parent");
				tran.parent=_target.transform;
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label ("        維持");
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			Vector3 _pos;	Vector3 _rota;	Vector3 _scale;
			if(_target){
				Undo.RegisterUndo (_target.transform, "Odls Transform child");
				_pos=_target.transform.localPosition;
				_rota=_target.transform.localEulerAngles;
				_scale=_target.transform.localScale;
				_target.transform.parent=tran;
				_target.transform.localPosition=_pos;
				_target.transform.localEulerAngles=_rota;
				_target.transform.localScale=_scale;
			}
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			if(_target){
				Undo.RegisterUndo (tran, "Odls Transform parent");
				_pos=tran.localPosition;
				_rota=tran.localEulerAngles;
				_scale=tran.localScale;
				tran.parent=_target.transform;
				tran.localPosition=_pos;
				tran.localEulerAngles=_rota;
				tran.localScale=_scale;
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label ("        對齊");
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			if(_target){
				Undo.RegisterUndo (_target.transform, "Odls Transform child");
				_target.transform.parent=tran;
				_target.transform.localPosition=Vector3.zero;
				_target.transform.localEulerAngles=Vector3.zero;
				_target.transform.localScale=Vector3.one;
			}
			_target = (GameObject)EditorGUILayout.ObjectField (null,typeof(GameObject),true);
			if(_target){
				Undo.RegisterUndo (tran, "Odls Transform parent");
				tran.parent=_target.transform;
				tran.transform.localPosition=Vector3.zero;
				tran.transform.localEulerAngles=Vector3.zero;
				tran.transform.localScale=Vector3.one;
			}
			GUILayout.EndHorizontal();
		}
	}
	bool snapFold;
	void Snap() {
		EditorGUI.indentLevel = 1;
		
		bool _snapFold = EditorGUILayout.Foldout (snapFold,"按住Ctrl的鎖點值");
		if (_snapFold!=snapFold) {
			snapFold=_snapFold;
			EditorPrefs.SetBool("Odls Transform snapFold", snapFold);
		}
		
		if(snapFold){
			EditorGUI.indentLevel = 2;
			GUILayout.Label(string.Format("Pos:({0},{1},{2})   Rota:{3}   Scale:{4}",
			                              Snaps[(int)E_SnapItem.posX],
			                              Snaps[(int)E_SnapItem.posY],
			                              Snaps[(int)E_SnapItem.posZ],
			                              Snaps[(int)E_SnapItem.rota],
			                              Snaps[(int)E_SnapItem.scale]),EditorStyles.largeLabel);
			if(GUILayout.Button("Setting")){
				EditorApplication.ExecuteMenuItem("Edit/Snap Settings...");
			}
		}
	}

	bool gridFold;
	void Grid() {
		EditorGUI.indentLevel = 1;

		GUILayout.BeginHorizontal();
		bool _gridFold = EditorGUILayout.Foldout (gridFold,"排列子物件");
				if (_gridFold!=gridFold) {
			gridFold=_gridFold;
			EditorPrefs.SetBool("Odls Transform gridFold", gridFold);
		}
		if (gridFold) {
			if (GUILayout.Button (gridWorld ? "World" : "Local")) {
				gridWorld = !gridWorld;
				EditorPrefs.SetBool ("Odls Transform gridWorld", gridWorld);
			}
		}
		GUILayout.EndHorizontal();

		if(gridFold){
			EditorGUI.indentLevel = 2;

			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
			EditorGUI.BeginChangeCheck ();
			gridPos.x=EditorGUILayout.FloatField("X",gridPos.x);
			gridPos.y=EditorGUILayout.FloatField("Y",gridPos.y);
			gridPos.z=EditorGUILayout.FloatField("Z",gridPos.z);
			if(EditorGUI.EndChangeCheck()){
				EditorPrefs.SetFloat ("Odls Transform gridX",gridPos.x);
				EditorPrefs.SetFloat ("Odls Transform gridY",gridPos.y);
				EditorPrefs.SetFloat ("Odls Transform gridZ",gridPos.z);
			}
			GUILayout.EndVertical();

			bool _needGridX=false;
			bool _needGridY=false;
			bool _needGridZ=false;

			GUILayout.BeginVertical();
			if(GUILayout.Button ("排列")){_needGridX=true;}
			if(GUILayout.Button ("排列")){_needGridY=true;}
			if(GUILayout.Button ("排列")){_needGridZ=true;}
			GUILayout.EndVertical();

			if(GUILayout.Button ("排列",GUILayout.Height(60))){
				_needGridX=true;
				_needGridY=true;
				_needGridZ=true;
			}

			GUILayout.EndHorizontal();

			if(_needGridX || _needGridY || _needGridZ){
				int _N=0;
				foreach(Transform _child in tran){
					Undo.RegisterUndo (_child, "Odls Transform grid");
					Vector3 _childPos;
					if(gridWorld){
						_childPos=tran.position;
						if(_needGridX){_childPos.x+=gridPos.x*_N;}
						if(_needGridY){_childPos.y+=gridPos.y*_N;}
						if(_needGridZ){_childPos.z+=gridPos.z*_N;}
						_child.transform.position=_childPos;
					}
					else{
						_childPos=_child.transform.localPosition;
						if(_needGridX){_childPos.x=gridPos.x*_N;}
						if(_needGridY){_childPos.y=gridPos.y*_N;}
						if(_needGridZ){_childPos.z=gridPos.z*_N;}
						_child.transform.localPosition=_childPos;
					}
					_N++;
				}
			}
		}
	}



	Vector3 DrawItem(Vector3 p_Vector,E_TranItem p_item) {
		GUILayout.BeginHorizontal();
		string _lable = lables [(int)p_item];
		GUILayout.Label (_lable,GUILayout.Width(50));
		p_Vector=EditorGUILayout.Vector3Field("",p_Vector);
		if (GUILayout.Button ("R",GUILayout.Width(30))) {
			p_Vector=(p_item==E_TranItem.scale)?Vector3.one:Vector3.zero;
		}
		if (GUILayout.Button ("C",GUILayout.Width(30))) {
			EditorPrefs.SetFloat("Odls Transform "+_lable+"X", p_Vector.x);
			EditorPrefs.SetFloat("Odls Transform "+_lable+"Y", p_Vector.y);
			EditorPrefs.SetFloat("Odls Transform "+_lable+"Z", p_Vector.z);
		} 
		if (GUILayout.Button ("P",GUILayout.Width(30))) {
			p_Vector=new Vector3(EditorPrefs.GetFloat("Odls Transform "+_lable+"X",0),
			                     EditorPrefs.GetFloat("Odls Transform "+_lable+"Y",0),
			                     EditorPrefs.GetFloat("Odls Transform "+_lable+"Z",0));
		}
		GUILayout.EndHorizontal();
		return p_Vector;
	}
	void OnEnable () {
		fold = EditorPrefs.GetBool ("Odls Transform fold", false);
		childFold = EditorPrefs.GetBool ("Odls Transform childFold", true);
		snapFold = EditorPrefs.GetBool ("Odls Transform snapFold", true);
		gridFold = EditorPrefs.GetBool ("Odls Transform gridFold", true);
		Snaps[(int)E_SnapItem.posX] = EditorPrefs.GetFloat ("MoveSnapX", 1);
		Snaps[(int)E_SnapItem.posY] = EditorPrefs.GetFloat ("MoveSnapY", 1);
		Snaps[(int)E_SnapItem.posZ] = EditorPrefs.GetFloat ("MoveSnapZ", 1);
		Snaps[(int)E_SnapItem.rota] = EditorPrefs.GetFloat ("RotationSnap", 15);
		Snaps[(int)E_SnapItem.scale] = EditorPrefs.GetFloat ("ScaleSnap", 0.1f);
		gridPos = new Vector3 (EditorPrefs.GetFloat ("Odls Transform gridX", 0),
		                       EditorPrefs.GetFloat ("Odls Transform gridY", 0),
		                       EditorPrefs.GetFloat ("Odls Transform gridZ", 0));
		gridWorld = EditorPrefs.GetBool ("Odls Transform gridWorld", false);
	}
}
#endif