using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManger : ManagerBase<SlimeBehaviourManger>
{

    Vector3 PlayerPosition;
    Vector3 ShopPosition;
    public bool ShopIsOpen = true;
    [SerializeField] float shopOpenDistence;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAtStep();
    }

    void UpdateAtStep()
    {
        if (Vector3.Distance(PlayerPosition, ShopPosition) < shopOpenDistence)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                UIManger.instance.OpenShopUI();
                ShopIsOpen = true;
            }
        }
    }

}
