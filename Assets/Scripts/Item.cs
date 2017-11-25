using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3
    };

    public ItemType type;
    public bool canFuse;
    public bool canDrop;
    
    public Item(ItemType type, bool canFuse, bool canDrop)
    {
        this.type = type;
        this.canFuse = canFuse;
        this.canDrop = canDrop;
    }

    ~Item()
    {

    }
}
