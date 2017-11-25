using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Scaler")]
public class ObjScaler : ObjCmdArgBase<Vector3> {
	[Header("Transform 類型")]
	public E_ObjCmdTransType transType = E_ObjCmdTransType.Local;
	[Header("預設值")]
	public Vector3 scale = Vector3.one;
	Vector3 lastScale;

	override protected bool CmdDefault(){
		return Cmd (scale);
	}
	override protected bool Cmd(Vector3 p_arg){
		switch(transType){
		case E_ObjCmdTransType.Local:
			lastScale = gameObject.transform.localScale;
			break;
		case E_ObjCmdTransType.Global:
			lastScale = gameObject.transform.lossyScale;
			break;
		default:
			lastScale = Vector3.one;
			break;
		}

		if (relatively) {
			p_arg = new Vector3(lastScale.x * p_arg.x,
			                    lastScale.y * p_arg.y,
			                    lastScale.z * p_arg.z);
		}
		if (lastScale.Equals(p_arg)) {
			return false;
		} else {
			switch(transType){
			case E_ObjCmdTransType.Local:
				gameObject.transform.localScale = p_arg;
				break;
			case E_ObjCmdTransType.Global:
				GameObjectExtend.SetLossyScale(gameObject.transform,p_arg);
				break;
			}
			return true;
		}
	}
	public override void Reset(){
		switch(transType){
		case E_ObjCmdTransType.Local:
			gameObject.transform.localScale = lastScale;
			break;
		case E_ObjCmdTransType.Global:
			GameObjectExtend.SetLossyScale(gameObject.transform,lastScale);
			break;
		}
	}
	[Button]public string setAsNowPosBut = "SetAsNowScale";
	public void SetAsNowScale(){
		switch(transType){
		case E_ObjCmdTransType.Local:
			scale = gameObject.transform.localScale;
			break;
		case E_ObjCmdTransType.Global:
			scale = gameObject.transform.lossyScale;
			break;
		}
	}
}
