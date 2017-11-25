using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ManagerBase<ItemManager> {

    [ReorderableList]
    public List<Item> PlayerItemList = new List<Item>();

    [ReorderableList]
    public List<Item> MapItemList = new List<Item>();

    private Dictionary<Item.ItemType, Item> itemTypeMap = new Dictionary<Item.ItemType, Item>();

    public ItemManager()
    {
        // enum order: empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3
        PlayerItemList.Add(new Item(Item.ItemType.empty, 0, false, false));
        PlayerItemList.Add(new Item(Item.ItemType.milk, 0, true, true));
        PlayerItemList.Add(new Item(Item.ItemType.oil, 0, true, true));
        PlayerItemList.Add(new Item(Item.ItemType.butter, 0, false, true));
        PlayerItemList.Add(new Item(Item.ItemType.acid, 0, true, true));
        PlayerItemList.Add(new Item(Item.ItemType.yogurt, 0, false, true));
        PlayerItemList.Add(new Item(Item.ItemType.poison, 0, false, true));
        PlayerItemList.Add(new Item(Item.ItemType.food1, 0, false, false));
        PlayerItemList.Add(new Item(Item.ItemType.food2, 0, false, false));
        PlayerItemList.Add(new Item(Item.ItemType.food3, 0, false, false));

        MapItemList.Add(new Item(Item.ItemType.empty, 0, false, false));
        MapItemList.Add(new Item(Item.ItemType.milk, 0, true, true));
        MapItemList.Add(new Item(Item.ItemType.oil, 0, true, true));
        MapItemList.Add(new Item(Item.ItemType.butter, 0, false, true));
        MapItemList.Add(new Item(Item.ItemType.acid, 0, true, true));
        MapItemList.Add(new Item(Item.ItemType.yogurt, 0, false, true));
        MapItemList.Add(new Item(Item.ItemType.poison, 0, false, true));
        MapItemList.Add(new Item(Item.ItemType.food1, 0, false, false));
        MapItemList.Add(new Item(Item.ItemType.food2, 0, false, false));
        MapItemList.Add(new Item(Item.ItemType.food3, 0, false, false));

        itemTypeMap.Add(Item.ItemType.empty, PlayerItemList[0]);
        itemTypeMap.Add(Item.ItemType.milk, PlayerItemList[1]);
        itemTypeMap.Add(Item.ItemType.oil, PlayerItemList[2]);
        itemTypeMap.Add(Item.ItemType.butter, PlayerItemList[3]);
        itemTypeMap.Add(Item.ItemType.acid, PlayerItemList[4]);
        itemTypeMap.Add(Item.ItemType.yogurt, PlayerItemList[5]);
        itemTypeMap.Add(Item.ItemType.poison, PlayerItemList[6]);
        itemTypeMap.Add(Item.ItemType.food1, PlayerItemList[7]);
        itemTypeMap.Add(Item.ItemType.food2, PlayerItemList[8]);
        itemTypeMap.Add(Item.ItemType.food3, PlayerItemList[9]);
        
    }

    bool Fuse(Item item1, Item item2)
    {
        bool isSuccess = false;

        if (item1.canFuse && item2.canFuse && item1.amount >= 0 && item2.amount >= 0)
        {
            if (item1.type != item2.type)
            {
                Item.ItemType resultType = (Item.ItemType)((int)item1.type + (int)item2.type);
                itemTypeMap[resultType].amount = 1;
                item1.amount = 0;
                item2.amount = 0;
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

    public bool DeFuse(Item fusedItem, Item dropItem)
    {
        bool isSuccess = false;

        if (fusedItem.amount >= 0 && dropItem.amount == 0)
        {
            Item.ItemType resultType = (Item.ItemType)((int)fusedItem.type - (int)dropItem.type);
            fusedItem.amount = 0;
            itemTypeMap[resultType].amount = 1;
            isSuccess = true;
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount");
        }

        return isSuccess;
    }

}
