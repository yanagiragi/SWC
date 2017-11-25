using UnityEngine;
using System.Collections;

public class ObjCmdDemo : MonoBehaviour {
	void Start () {}
	void Update () {}
	public void MoveUp (GameObject p_obj) {
		ObjCmdManager.NewCmd<ObjMover,Vector3>(p_obj,new Vector3(0,10,0),true,1f);
//		ObjCmdManager.NewCmd<ObjActiver,bool>(p_obj,false,false,2f);
//		ObjCmdManager.NewCmd<ObjActiver,bool>(p_obj,true,false,3f);
//		ObjCmdManager.NewCmd<ObjDeleter>(p_obj,false,4f);
	}
}
