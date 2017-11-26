using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : ManagerBase<RestartManager> {
	void Start () {
		ReStart ();
	}
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)){
			ReStart ();
		}
	}
	static public void ReStart(){
		DungeonManager.ReStart ();
		PlayerManager.ReStart ();
        StepManager.ReStart();
	}

	static public void ChangeMap(){
		DungeonManager.ChangeMap ();
		PlayerManager.ChangeMap ();
	}
}
