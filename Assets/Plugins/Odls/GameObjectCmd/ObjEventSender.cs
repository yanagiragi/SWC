using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Event Sender")]
public class ObjEventSender : ObjCmdBase {
	public UnityEvent OnCmdEvent;
	override protected bool CmdDefault(){
		if (OnCmdEvent == null) {
			return false;
		} else {
			OnCmdEvent.Invoke();
			return true;
		}
	}
}
