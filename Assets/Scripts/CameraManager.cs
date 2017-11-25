using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : ManagerBase<CameraManager> {
    public Vector3 Offset;
    void Update () {
        Camera.main.gameObject.transform.position = PlayerManager.instance.playerInstance.transform.position + Offset;
    }
}
