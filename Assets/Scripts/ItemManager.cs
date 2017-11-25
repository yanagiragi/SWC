using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : BaseManager<ItemManager> {

    private List<Item> items = new List<Item>();

    public ItemManager()
    {
        items.Add(new Item(Item.ItemType.milk, 0, true, true));
        items.Add(new Item(Item.ItemType.oil, 0, true, true));
        items.Add(new Item(Item.ItemType.acid, 0, true, true));
        items.Add(new Item(Item.ItemType.food1, 0, false, false));
        items.Add(new Item(Item.ItemType.food1, 0, false, false));
        items.Add(new Item(Item.ItemType.food3, 0, false, false));
    }

    bool FuseItem(Item item1, Item item2)
    {
        bool result = item2.Fuse(item1);

        if (result)
        {
            item1.ClearAmount();
        }

        return result;
    }
}
