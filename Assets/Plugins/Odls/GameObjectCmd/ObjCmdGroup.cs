using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[AddComponentMenu("Odls Obj Cmd/Cmd Group")]
public class ObjCmdGroup : ObjCmdArgBase<int> {
	public List<ObjCmdBase> cmdList;
	int index = -1;
	int len = 0;
	override protected bool CmdDefault(){
		return Cmd (0);
	}
	override protected bool Cmd(int p_arg){
		len = cmdList.Count;
		if (index >= 0) {
			cmdList[index].OnCmdEnd -= OnLastCmdEnd;
			cmdList [index].CancelCmd();	
		}
		if (p_arg < len) {
			index = p_arg-1;
			NextCmd();
		} else {
			index = -1;
		}
		return false;
	}
	void NextCmd(){
		index++;
		if (index < len) {
			cmdList [index].OnCmdEnd += OnLastCmdEnd;
			cmdList [index].DoCmd ();			
		} else {
			index = -1;
		}
	}
	void OnLastCmdEnd(){
		cmdList[index].OnCmdEnd -= OnLastCmdEnd;
		NextCmd ();
	}
	public override void Reset(){
		int f;
		if(index >= 0){
			for(f=index; f>=0; f--){
				cmdList [f].Reset();
			}
		}else{
			for(f=0; f<len; f++){
				cmdList [f].Reset();
			}
		}
	}
}
