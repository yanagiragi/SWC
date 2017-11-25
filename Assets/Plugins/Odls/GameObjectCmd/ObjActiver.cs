using UnityEngine;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Activer")]
public class ObjActiver : ObjCmdArgBase<bool> {
	[Header("反向結果")]
	public bool inverse;
	[Header("預設值")]
	public bool active = false;

	bool _lastValue;
	override protected bool CmdDefault(){
		return Cmd (active);
	}
	override protected bool Cmd(bool p_arg){
		_lastValue = gameObject.activeSelf;
		if (relatively) {
			p_arg = _lastValue ^ p_arg;
		}
		if (_lastValue == p_arg ^ inverse) {
			return false;
		} else {
			gameObject.SetActive (p_arg ^ inverse);
			return true;
		}
	}
	public override void Reset(){
		gameObject.SetActive (_lastValue);
	}
}
