using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Enabler")]
public class ObjEnabler : ObjCmdArgBase<bool> {
	public enum E_ObjEnablerType{Script,Render,UI};
	[Header("反向結果")]
	public bool inverse;
	[Header("預設值")]
	public bool enable = false;
	[Header("類型")]
	public E_ObjEnablerType type;
	[Header("對象")]
	[NullAlarm]public Component component = null;

	bool _lastValue;

	bool componentEnabled{
		get{
			switch(type){
			case E_ObjEnablerType.Script:
				return ((MonoBehaviour)component).enabled;
				break;
			case E_ObjEnablerType.Render:
				return ((Renderer)component).enabled;
				break;
			case E_ObjEnablerType.UI:
				return ((Graphic)component).enabled;
				break;
			default:
				return false;
				break;
			}
		}
		set{
			switch(type){
			case E_ObjEnablerType.Script:
				((MonoBehaviour)component).enabled = value;
				break;
			case E_ObjEnablerType.Render:
				((Renderer)component).enabled = value;
				break;
			case E_ObjEnablerType.UI:
				((Graphic)component).enabled = value;
				break;
			default:
				
				break;
			}
		}
	}

	override protected bool CmdDefault(){
		return Cmd (enable);
	}
	override protected bool Cmd(bool p_arg){
		_lastValue = componentEnabled;
		if (relatively) {
			p_arg = _lastValue ^ p_arg;
		}
		if (_lastValue == p_arg ^ inverse) {
			return false;
		} else {
			componentEnabled = p_arg ^ inverse;
			return true;
		}
	}
	public override void Reset(){
		componentEnabled = _lastValue;
	}
}
