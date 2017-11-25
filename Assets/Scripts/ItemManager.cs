using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ManagerBase<ItemManager> {

    [ReorderableList]
    public List<Item> ItemList = new List<Item>();
    
    public ItemManager()
    {
        ItemList.Add(new Item(Item.ItemType.empty, false, false));
        ItemList.Add(new Item(Item.ItemType.milk, true, true));
        ItemList.Add(new Item(Item.ItemType.oil, true, true));
        ItemList.Add(new Item(Item.ItemType.butter, false, true));
        ItemList.Add(new Item(Item.ItemType.acid, true, true));
        ItemList.Add(new Item(Item.ItemType.yogurt, false, true));
        ItemList.Add(new Item(Item.ItemType.poison, false, true));
        ItemList.Add(new Item(Item.ItemType.food1, false, false));
        ItemList.Add(new Item(Item.ItemType.food2, false, false));
        ItemList.Add(new Item(Item.ItemType.food3, false, false));
    }

    public static Item GetItemData(Item.ItemType type)
    {
        return ItemManager.instance.ItemList[(int)type];
    }

}
