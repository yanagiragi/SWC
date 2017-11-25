using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviourManger : ManagerBase<SlimeBehaviourManger>
{
    [SerializeField] Item.ItemType currentBehaviourType;
    [SerializeField] GameObject bait;

    [SerializeField] int BloodIncrease;
    static public int AbilityCoolDown = 0;
    bool isCoolDowm;
    public static bool HaveCoolDown = false;
    [SerializeField] int CoolDownTime;
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
            //currentBehaviourType();
            if (AbilityCoolDown > 1 && HaveCoolDown)
            {
                AbilityCoolDown -= 1;
                isCoolDowm = true;
            }
            DoBehaviourEffect();
            if (AbilityCoolDown == 0 && HaveCoolDown)
            {
                isCoolDowm = false;
                AbilityCoolDown = CoolDownTime;
            }

            Debug.Log(DungeonManager.GetMapData(playerPosition).pos);
        }
    }

    /* Item.ItemType currentBehaviourType()
    {
        if (PlayerManager.instance.PlayerItemList.Contains(1))
        {
            HaveCoolDown = true;
            return Item.ItemType.milk;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(2))
        {
            HaveCoolDown = false;
            return Item.ItemType.oil;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(3))
        {
            HaveCoolDown = true;
            return Item.ItemType.butter;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(4))
        {
            HaveCoolDown = true;
            return Item.ItemType.acid;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(5))
        {
            HaveCoolDown = true;
            return Item.ItemType.yogurt;
        }
        else if (PlayerManager.instance.PlayerItemList.Contains(6))
        {
            HaveCoolDown = true;
            return Item.ItemType.poison;
        }
        else
        {
            HaveCoolDown = false;
            return Item.ItemType.empty;
        }
    } */

    public void UpdatePlayerPosition(Vector3 playerPositionGet)
    {
        playerPosition = new Vector2(Mathf.Round(playerPositionGet.x), Mathf.Round(playerPositionGet.z));
    }

    void DoBehaviourEffect()
    {

        switch (currentBehaviourType)
        {
            case Item.ItemType.milk:
                HaveCoolDown = true;
                MilkIncreaseBlood();
                break;
            case Item.ItemType.acid:
                HaveCoolDown = true;
                AcidMeltWall();
                break;
            case Item.ItemType.oil:
                HaveCoolDown = false;
                WalkOnWater();
                break;
            case Item.ItemType.butter:
                HaveCoolDown = true;
                if (!isCoolDowm)
                    CrossWall();
                break;
            case Item.ItemType.poison:
                HaveCoolDown = true;
                if (!isCoolDowm)
                    PoisonKill();
                break;
            case Item.ItemType.yogurt:
                HaveCoolDown = true;
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

