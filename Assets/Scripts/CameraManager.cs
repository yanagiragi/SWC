using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : ManagerBase<CameraManager> {
	public Transform cmrCenter;
    void Update () {
		cmrCenter.position = PlayerManager.instance.playerInstance.transform.position;
    }
}
