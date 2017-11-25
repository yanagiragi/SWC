using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3
    };

    public ItemType type;
    public int amount;
    public bool canFuse;
    public bool canDrop;
    
    public Item(ItemType type, int amount, bool canFuse, bool canDrop)
    {
        this.type = type;
        this.amount = amount;
        this.canFuse = canFuse;
        this.canDrop = canDrop;
    }

    ~Item()
    {

    }
}
