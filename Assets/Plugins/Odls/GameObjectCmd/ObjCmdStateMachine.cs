using UnityEngine;
using System.Collections;

public enum E_ObjCmdStateMachineType{None,StateEnter,StateExit,StateUpdate,StateMove,StateIK}
public class ObjCmdStateMachine : StateMachineBehaviour{
	[LockInInspector]public ObjCmdBase[] cmd;
	public E_ObjCmdStateMachineType type;
	[Header("延遲時間，-1 時使用預設值")]
	public float delayTime = -1;

	public bool GetCmd(Animator p_anim){
		if (cmd == null) {
			cmd = p_anim.GetComponents<ObjCmdBase> ();
		} else if (cmd.Length <= 0) {
			cmd = p_anim.GetComponents<ObjCmdBase> ();
		} else {
			return true;
		}

		if (cmd == null) {
			Debug.LogError(p_anim.gameObject.name + " use ObjCmdStateMachine, But No ObjCmd");
			return false;
		} else if (cmd.Length <= 0) {
			Debug.LogError(p_anim.gameObject.name + " use ObjCmdStateMachine, But No ObjCmd");
			return false;
		} else {
			return true;
		}
	}
	public void DoCmd(Animator p_anim){
		if (GetCmd (p_anim)) {
			int f;
			int len = cmd.Length;
			for (f=0; f<len; f++) {
				cmd [f].DoCmd (delayTime);
			}
		}
	}
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
		if (type == E_ObjCmdStateMachineType.StateEnter) {
			DoCmd(animator);
		}
	}
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
		if (type == E_ObjCmdStateMachineType.StateExit) {
			DoCmd(animator);
		}
	}
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
		if (type == E_ObjCmdStateMachineType.StateUpdate) {
			DoCmd(animator);
		}
	}
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
		if (type == E_ObjCmdStateMachineType.StateMove) {
			DoCmd(animator);
		}
	}
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
		if (type == E_ObjCmdStateMachineType.StateIK) {
			DoCmd(animator);
		}
	}
}
