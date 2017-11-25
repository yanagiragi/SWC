using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Mover")]
public class ObjMover : ObjCmdArgBase<Vector3> {
	[Header("Transform 類型")]
	public E_ObjCmdTransType transType = E_ObjCmdTransType.Local;
	[Header("預設值")]
	public Vector3 offset;
	Vector3 lastPos;
	override protected bool CmdDefault(){
		return Cmd (offset);
	}
	override protected bool Cmd(Vector3 p_arg){

		switch(transType){
		case E_ObjCmdTransType.Local:
			lastPos = gameObject.transform.localPosition;
			break;
		case E_ObjCmdTransType.Global:
			lastPos = gameObject.transform.position;
			break;
		default:
			lastPos = Vector3.zero;
			break;
		}

		if (relatively) {
			p_arg = lastPos + p_arg;
		}
		if (lastPos.Equals(p_arg)) {
			return false;
		} else {
			switch(transType){
			case E_ObjCmdTransType.Local:
				gameObject.transform.localPosition = p_arg;
				break;
			case E_ObjCmdTransType.Global:
				gameObject.transform.position = p_arg;
				break;
			}
			return true;
		}
	}
	public override void Reset(){
		switch(transType){
		case E_ObjCmdTransType.Local:
			gameObject.transform.localPosition = lastPos;
			break;
		case E_ObjCmdTransType.Global:
			gameObject.transform.position = lastPos;
			break;
		}
	}
	[Button]public string setAsNowPosBut = "SetAsNowPos";
	public void SetAsNowPos(){
		switch(transType){
		case E_ObjCmdTransType.Local:
			offset = gameObject.transform.localPosition;
			break;
		case E_ObjCmdTransType.Global:
			offset = gameObject.transform.position;
			break;
		}
	}
}
