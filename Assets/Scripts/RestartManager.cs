using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : ManagerBase<RestartManager> {
	void Start () {
		ReStart ();
	}
	void Update () {
		
	}
	static public void ReStart(){
		DungeonManager.ReStart ();
		PlayerManager.ReStart ();
        StepManager.ReStart();
	}
}
