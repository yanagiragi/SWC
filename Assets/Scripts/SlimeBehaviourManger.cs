using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviourManger : ManagerBase<SlimeBehaviourManger>
{
    [SerializeField] Item.ItemType currentBehaviourType;
    [SerializeField] int BloodIncrease;
    static public int abilityCoolDown = 0;
    bool isCoolDowm;
    public static bool haveCoolDown = false;
    [SerializeField] public int CoolDownTime;
    [SerializeField] int WaterDamage;
    [SerializeField] GameObject Yogurt;
    Vector2 playerPosition;

    void Awake()
    {
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetcurrentBehaviourType();
            if (!haveCoolDown)
            {
                DoBehaviourEffect();
                UIManger.instance.AbilityCoolDown(0);
            }
            else
            {
                if (abilityCoolDown > 0)
                {
                    UIManger.instance.AbilityCoolDown((float)abilityCoolDown / (float)CoolDownTime);
                    abilityCoolDown -= 1;
                    isCoolDowm = true;
                    
                }
                else if(abilityCoolDown == 0)
                {
                    isCoolDowm = false;
                    DoBehaviourEffect();
                    abilityCoolDown = CoolDownTime;
                    UIManger.instance.AbilityCoolDown(0);
                }
            }

        }
    }

    void GetcurrentBehaviourType()
    {
        haveCoolDown = true;
        return;
        if (PlayerManager.instance.PlayerItemList.Contains(1))
        {
            haveCoolDown = true;
            //currentBehaviourType = Item.ItemType.milk;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(2))
        {
            haveCoolDown = false;
            //currentBehaviourType = Item.ItemType.oil;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(3))
        {
            haveCoolDown = true;
            //currentBehaviourType = Item.ItemType.butter;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(4))
        {
            haveCoolDown = true;
            //currentBehaviourType = Item.ItemType.acid;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(5))
        {
            haveCoolDown = true;
            //currentBehaviourType = Item.ItemType.yogurt;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(6))
        {
            haveCoolDown = true;
            //currentBehaviourType = Item.ItemType.poison;
        }
        else
        {
            haveCoolDown = false;
            //currentBehaviourType = Item.ItemType.empty;
        }
    }

    public void UpdatePlayerPosition(Vector3 playerPositionGet)
    {
        playerPosition = new Vector2(Mathf.Round(playerPositionGet.x), Mathf.Round(playerPositionGet.z));
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
                if (!isCoolDowm)
                    CrossWall();
                break;
            case Item.ItemType.poison:
                if (!isCoolDowm)
                    PoisonKill();
                break;
            case Item.ItemType.yogurt:
                PutYogurt();
                break;
        }
    }

    void MilkIncreaseBlood()
    {
        PlayerManager.IncreaseHealth(BloodIncrease);
    }

    public void AcidMeltWall()
    {
        DungeonMapData dungeonMapData = DungeonManager.GetMapData(playerPosition);
        Debug.Log(dungeonMapData.cubeType);
        if (dungeonMapData.cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Debug.Log("AcidMeltWall");
            DungeonManager.ChangeCubeType(playerPosition, E_DUNGEON_CUBE_TYPE.NONE);

        }
    }

    public void WalkOnWater()
    {
        DungeonMapData dungeonMapData = DungeonManager.GetMapData(playerPosition);
        if (dungeonMapData.cubeType == E_DUNGEON_CUBE_TYPE.WATER)
        {
            if (currentBehaviourType != Item.ItemType.butter)
            {
                PlayerManager.DecreaseHealth(WaterDamage);
                Debug.Log(PlayerManager.instance.health);
            }
        }
    }

    void CrossWall()
    {
        try
        {
            if (DungeonManager.GetMapData(playerPosition + new Vector2(1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                PlayerManager.instance.playerInstance.transform.position = playerPosition + new Vector2(1, 1);
                return;
            }
        }
        catch { }
        try
        {
            if (DungeonManager.GetMapData(playerPosition + new Vector2(-1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                PlayerManager.instance.playerInstance.transform.position = playerPosition + new Vector2(-1, 1);
                return;
            }
        }
        catch { }
        try
        {
            if (DungeonManager.GetMapData(playerPosition + new Vector2(1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                PlayerManager.instance.playerInstance.transform.position = playerPosition + new Vector2(1, -1);
                return;
            }
        }
        catch { }
        try
        {
            if (DungeonManager.GetMapData(playerPosition + new Vector2(-1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER)
            {
                PlayerManager.instance.playerInstance.transform.position = playerPosition + new Vector2(-1, -1);
                return;
            }
        }
        catch { }
    }

    void PoisonKill()
    {

    }

    void PutYogurt()
    {
        GameObject clone = Instantiate(Yogurt, PlayerManager.instance.playerInstance.transform.position, PlayerManager.instance.playerInstance.transform.rotation);
        PlayerManager.instance.yogurtInstance = clone;
    }

}

