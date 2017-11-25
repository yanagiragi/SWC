using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviourManger : ManagerBase<SlimeBehaviourManger>
{
    /*測試用變數 */
    [SerializeField] int currentGird_x;
    [SerializeField] int currentGird_y;
    [SerializeField] int PlayerBlood;
    /* */

    [SerializeField] Item.ItemType currentBehaviourType;
    [SerializeField] GameObject bait;

    [SerializeField] int BloodIncrease;
    int AbilityCoolDown = 0;
    bool isCoolDowm;
    [SerializeField] int CoolDownTime;
    Vector3 playerPosition;
    //DungeonMapData dungeonMapData;

    void Awake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateAtStep();
        }
    }

    void UpdateAtStep()
    {
        if (AbilityCoolDown == 0)
        {
            DoBehaviourEffect();
            AbilityCoolDown = CoolDownTime;
        }
        else
        {
            AbilityCoolDown -= 1; 
        }
        Debug.Log(DungeonManager.GetMapData(PlayerManager.instance.playerInstance.transform.position).cubeType);
    }

    public void UpdatePlayerPosition(Vector3 PlayerPosition)
    {
        playerPosition = PlayerPosition;
    }

    void DoBehaviourEffect()
    {
        
        switch (currentBehaviourType)
        {
            case Item.ItemType.milk:
                MilkIncreaseBlood();
                break;
            case Item.ItemType.acid:
                AcidMeltWall();
                break;
            case Item.ItemType.oil:
                WalkOnWater();
                break;
            case Item.ItemType.butter:
                CrossWall();
                break;
            case Item.ItemType.poison:
                PoisonKill();
                break;
            case Item.ItemType.yogurt:

                break;
        }
    }

    void MilkIncreaseBlood()
    {
        PlayerBlood += BloodIncrease;
    }

    public void AcidMeltWall()
    {
        DungeonMapData dungeonMapData = DungeonManager.GetMapData(PlayerManager.instance.playerInstance.transform.position);
        if (dungeonMapData.cubeType == E_DUNGEON_CUBE_TYPE.LEN)
        {
            Debug.Log("牆被融化");
        }
    }

    void WalkOnWater()
    {
        DungeonMapData dungeonMapData = DungeonManager.GetMapData(currentGird_x, currentGird_y);
        if (dungeonMapData.cubeType == E_DUNGEON_CUBE_TYPE.WATER)
        {
            Debug.Log("可以走過水");
        }
    }

    void CrossWall()
    {
        try
        {
            if (DungeonManager.GetMapData(currentGird_x + 1, currentGird_y + 1).cubeType == E_DUNGEON_CUBE_TYPE.NONE || DungeonManager.GetMapData(currentGird_x + 1, currentGird_y + 1).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                Debug.Log("玩家移動");
                return;
            }
        }
        catch{}
        try
        {
            if (DungeonManager.GetMapData(currentGird_x - 1, currentGird_y + 1).cubeType == E_DUNGEON_CUBE_TYPE.NONE || DungeonManager.GetMapData(currentGird_x - 1, currentGird_y + 1).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                Debug.Log("玩家移動");
                return;
            }
        }
        catch{}
        try
        {
            if (DungeonManager.GetMapData(currentGird_x + 1, currentGird_y - 1).cubeType == E_DUNGEON_CUBE_TYPE.NONE || DungeonManager.GetMapData(currentGird_x + 1, currentGird_y - 1).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                Debug.Log("玩家移動");
                return;
            }
        }
        catch{}
        try
        {
            if (DungeonManager.GetMapData(currentGird_x - 1, currentGird_y - 1).cubeType == E_DUNGEON_CUBE_TYPE.NONE || DungeonManager.GetMapData(currentGird_x + 1, currentGird_y + 1).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                Debug.Log("玩家移動");
                return;
            }
        }
        catch{}
    }

    void PoisonKill()
    {
        
    }

    void PutBait()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

}

