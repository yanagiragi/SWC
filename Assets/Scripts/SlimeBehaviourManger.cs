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

    public int milkUseLimit = 2; // Actually Can use milkUseLimit + 1 times, then lost
    public int milkInterval = 5;
    public int milkCount = 0;
    public int trapMinusHealth = 5;

    bool abilityIsUse = false;

    DungeonMapData nextStepData;

    void Awake()
    {
    }

    public void WalkOnTrap()
    {
        PlayerManager.DecreaseHealth(trapMinusHealth);
    }

    public void UpdateAtStep()
    {
        Debug.Log("Step");
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
            else if (abilityCoolDown == 0)
            {
                abilityIsUse = false;
                isCoolDowm = false;
                DoBehaviourEffect();

            }

            if (abilityIsUse)
            {
                isCoolDowm = true;
                abilityCoolDown = CoolDownTime;
                UIManger.instance.AbilityCoolDown(1);
            }
        }
    }

    void GetcurrentBehaviourType()
    {
        haveCoolDown = false;
        if (currentBehaviourType == Item.ItemType.butter)
        {
            haveCoolDown = true;
        }
        currentBehaviourType = PlayerManager.instance.slimeMode;
    }

    public void UpdatePlayerPosition(Vector3 playerPositionGet)
    {
        playerPosition = new Vector2(Mathf.Round(playerPositionGet.x), Mathf.Round(playerPositionGet.z));
    }

    public void UpdatePlayerPositionRT()
    {
        playerPosition = new Vector2(Mathf.Round(PlayerManager.instance.playerInstance.transform.position.x), Mathf.Round(PlayerManager.instance.playerInstance.transform.position.z));
    }

    private void Update()
    {
        UpdatePlayerPositionRT();
        if (Input.GetKeyDown(KeyCode.J) && currentBehaviourType == Item.ItemType.yogurt)
        {
            Debug.Log("Put Yogurt!");
            PutYogurt();
        }

        if (currentBehaviourType == Item.ItemType.butter && isCoolDowm == false)
        {
            CrossWall();
        }
    }

    void DoBehaviourEffect()
    {
        switch (currentBehaviourType)
        {
            case Item.ItemType.milk:
                MilkIncreaseBlood();
                break;
            case Item.ItemType.acid:
                //AcidMeltWall();
                break;
            case Item.ItemType.oil:
                //WalkOnWater();
                break;
            case Item.ItemType.poison:
                if (!isCoolDowm)
                    ;//PoisonKill();
                break;
            case Item.ItemType.yogurt:
                //PutYogurt();
                break;
        }
    }

    void MilkIncreaseBlood()
    {
        if (PlayerManager.instance.PlayerItemList[(int)Item.ItemType.milk] > 0)
        {
            if (milkCount % milkInterval == 0)
            {
                Debug.Log("Increase Health: " + BloodIncrease);
                PlayerManager.IncreaseHealth(BloodIncrease);
            }

            if (milkCount >= milkInterval * milkUseLimit)
            {
                Debug.Log("Lost Milk!");
                PlayerManager.instance.PlayerItemList[(int)Item.ItemType.milk] = 0;
                milkCount = 0;
                PlayerManager.instance.SetSlimeMode(Item.ItemType.empty);
                return;
            }

            ++milkCount;
        }
    }

    public void AcidMeltWall()
    {
        if (nextStepData.cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Debug.Log("AcidMeltWall");
			SoundManager.instance.do_acidDestroy ();
            DungeonManager.ChangeCubeType(nextStepData.pos, E_DUNGEON_CUBE_TYPE.NONE);
        }
    }

    public void WalkOnWater()
    {
        //DungeonMapData dungeonMapData = DungeonManager.GetMapData(playerPosition);
        if (nextStepData == null)
        {
            ;
        }
        if (nextStepData.cubeType == E_DUNGEON_CUBE_TYPE.WATER)
        {
            if (currentBehaviourType != Item.ItemType.oil)
            {
                PlayerManager.DecreaseHealth(WaterDamage);
                Debug.Log(PlayerManager.instance.health);
            }
        }
    }

    void CrossWall()
    {
        int Count = 0;
        if (DungeonManager.GetMapData(playerPosition + new Vector2(0, 1)).cubeType == E_DUNGEON_CUBE_TYPE.WALL ||
        DungeonManager.GetMapData(playerPosition + new Vector2(0, 1)).cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Count++;
        }

        if (DungeonManager.GetMapData(playerPosition + new Vector2(0, -1)).cubeType == E_DUNGEON_CUBE_TYPE.WALL ||
DungeonManager.GetMapData(playerPosition + new Vector2(0, -1)).cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Count++;
        }
        if (DungeonManager.GetMapData(playerPosition + new Vector2(1, 0)).cubeType == E_DUNGEON_CUBE_TYPE.WALL ||
DungeonManager.GetMapData(playerPosition + new Vector2(1, 0)).cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Count++;
        }
        if (DungeonManager.GetMapData(playerPosition + new Vector2(-1, 0)).cubeType == E_DUNGEON_CUBE_TYPE.WALL ||
DungeonManager.GetMapData(playerPosition + new Vector2(-1, 0)).cubeType == E_DUNGEON_CUBE_TYPE.EARTH)
        {
            Count++;
        }
        if (Input.GetKeyDown(KeyCode.J) && Count > 1)
        {
            Debug.Log(playerPosition);
            if (DungeonManager.GetMapData(playerPosition + new Vector2(1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.TRAP)
            {
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                {
                    PlayerManager.instance.destination += new Vector3(1, 0, 1);
                    PlayerManager.instance.Move();
                    abilityIsUse = true;
					SoundManager.instance.do_teleport ();
                    return;
                }
            }

            if (DungeonManager.GetMapData(playerPosition + new Vector2(-1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, 1)).cubeType == E_DUNGEON_CUBE_TYPE.TRAP)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                {
                    PlayerManager.instance.destination += new Vector3(-1, 0, 1);
                    PlayerManager.instance.Move();
                    abilityIsUse = true;
					SoundManager.instance.do_teleport ();
                    return;
                }
            }



            if (DungeonManager.GetMapData(playerPosition + new Vector2(1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER ||
            DungeonManager.GetMapData(playerPosition + new Vector2(1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.TRAP)
            {
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
                {
                    PlayerManager.instance.destination += new Vector3(1, 0, -1);
                    PlayerManager.instance.Move();
                    abilityIsUse = true;
					SoundManager.instance.do_teleport ();
                    return;
                }
            }


            if (DungeonManager.GetMapData(playerPosition + new Vector2(-1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.NONE ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.WATER ||
            DungeonManager.GetMapData(playerPosition + new Vector2(-1, -1)).cubeType == E_DUNGEON_CUBE_TYPE.TRAP)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
                {
                    PlayerManager.instance.destination += new Vector3(-1, 0, -1);
                    PlayerManager.instance.Move();
                    abilityIsUse = true;
					SoundManager.instance.do_teleport ();
                    return;
                }
            }
        }

    }

    void PoisonKill()
    {

    }

    void PutYogurt()
    {
        if (PlayerManager.instance.PlayerItemList[(int)Item.ItemType.yogurt] > 0)
        {
            PlayerManager.instance.putYogurt();
        }
    }

    public void GetNextStep(DungeonMapData getNextStepData)
    {
        Debug.Log(getNextStepData);
        nextStepData = getNextStepData;
    }

}

