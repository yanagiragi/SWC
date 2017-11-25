using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager> {

    [ReorderableList]
    public Dictionary<Item.ItemType, int> PlayerItemList = new Dictionary<Item.ItemType, int>();

    PlayerManager()
    {
        // enum order: empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3
        PlayerItemList.Add(Item.ItemType.empty,  0);
        PlayerItemList.Add(Item.ItemType.milk,   0);
        PlayerItemList.Add(Item.ItemType.oil,    0);
        PlayerItemList.Add(Item.ItemType.butter, 0);
        PlayerItemList.Add(Item.ItemType.acid,   0);
        PlayerItemList.Add(Item.ItemType.yogurt, 0);
        PlayerItemList.Add(Item.ItemType.poison, 0);
        PlayerItemList.Add(Item.ItemType.food1,  0);
        PlayerItemList.Add(Item.ItemType.food2,  0);
        PlayerItemList.Add(Item.ItemType.food3,  0);
    }

    public bool Fuse(Item.ItemType itemType1, Item.ItemType itemType2)
    {
        bool isSuccess = false;

        bool item1CanFuse = ItemManager.instance.ItemList[(int)itemType1].canFuse;
        bool item2CanFuse = ItemManager.instance.ItemList[(int)itemType2].canFuse;
        bool item1AmmountOverZero = PlayerItemList[itemType1] >= 0;
        bool item2AmmountOverZero = PlayerItemList[itemType2] >= 0;

        if (item1CanFuse && item2CanFuse && item1AmmountOverZero && item2AmmountOverZero)
        {
            if (itemType1 != itemType2)
            {
                Item.ItemType resultType = (Item.ItemType)((int)itemType1 + (int)itemType2);
                PlayerItemList[resultType] = 1;
                PlayerItemList[itemType1] = 0;
                PlayerItemList[itemType2] = 0;
                isSuccess = true;
            }
            else
            {
                Debug.LogError("Error occues when fusing with type");
            }
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount");
        }

        return isSuccess;
    }

    public bool DeFuse(Item.ItemType FusedItemType, Item.ItemType dropItemType)
    {
        bool isSuccess = false;

        if (PlayerItemList[FusedItemType] >= 0 && PlayerItemList[dropItemType] == 0)
        {
            Item.ItemType resultType = (Item.ItemType)((int)FusedItemType - (int)dropItemType);
            PlayerItemList[resultType] = 1;
            PlayerItemList[FusedItemType] = 0;
            PlayerItemList[dropItemType] = 0;
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount");
        }

        return isSuccess;
    }

    void UpdateAtStep()
    {

    }

    void Walk()
    {
        
    }

    void UseItem(int itemID)
    {

    }

}
