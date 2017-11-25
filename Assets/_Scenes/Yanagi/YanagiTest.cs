using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YanagiTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Press");

            PlayerManager.instance.PlayerItemList[Item.ItemType.milk] = 1;
            PlayerManager.instance.PlayerItemList[Item.ItemType.acid] = 1;

            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.yogurt]);
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.milk]);
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.acid]);

            Debug.Log(PlayerManager.instance.Fuse(Item.ItemType.acid, Item.ItemType.milk));

            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.yogurt]);
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.milk]);
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.acid]);

            Debug.Log(PlayerManager.instance.DeFuse(Item.ItemType.yogurt, Item.ItemType.milk));
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.milk]);
            Debug.Log(PlayerManager.instance.PlayerItemList[Item.ItemType.acid]);
        }
	}
}
