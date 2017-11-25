using UnityEngine;
using System.Collections;

[AddComponentMenu("Odls Obj Cmd/Deleter")]
public class ObjDeleter : ObjCmdBase {
	override protected bool CmdDefault(){
		Destroy (gameObject);
		return true;
	}
}