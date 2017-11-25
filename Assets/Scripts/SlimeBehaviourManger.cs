using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeBehaviourManger : ManagerBase<slimeBehaviourManger>
{
    Item.ItemType currentBehaviourType;
	[SerializeField]int bloodIncreaseAmount;
    [SerializeField]GameObject bait;

    void UpdateAtStep()
    {
        DoBehaviourEffect();
    }

    void DoBehaviourEffect()
    {
        switch(currentBehaviourType)
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

    }

    void AcidMeltWall()
    {
        
    }

    void WalkOnWater()
    {
    }

    void CrossWall()
    {
        
    }

    void PoisonKill()
    {
        
    }

    void PutBait()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bait);
        }
    }

}

