using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        milk, oil, acid, food1, food2, food3, butter, yogurt, poison
    };

    private ItemType type;
    private int amount;
    private bool canFuse;
    private bool canDrop;
    
    public Item(ItemType type, int amount, bool canFuse, bool canDrop)
    {
        this.type = type;
        this.amount = amount;
        this.canFuse = canFuse;
        this.canDrop = canDrop;
    }

    public bool AddAmount()
    {
        int AddAmount = 1;
        bool isSuccess = false;

        if (this.amount >= 1)
        {
            switch (this.type)
            {
                case ItemType.food1:
                case ItemType.food2:
                case ItemType.food3:
                    this.amount += AddAmount;
                    isSuccess = true;
                    break;
                default:
                    Debug.LogError(this.type + " Exceed Amount.");
                    break;
            }
        }
        else
        {
            this.amount = AddAmount;
            isSuccess = true;
        }

        return isSuccess;
    }

    public void ClearAmount()
    {
        this.amount = 0;
    }

    public bool Fuse(Item target)
    {
        bool isSuccess = false;

        if(this.canFuse && this.amount >= 0 && target.amount >= 0)
        {
            if (this.type == ItemType.milk)
            {
                switch(target.type)
                {
                    case ItemType.acid:
                        this.type = ItemType.yogurt;
                        isSuccess = true;
                        break;

                    case ItemType.milk:
                        // Do Nothing
                        break;

                    case ItemType.oil:
                        this.type = ItemType.butter;
                        isSuccess = true;
                        break;
                }

            }
            else if (this.type == ItemType.acid)
            {
                switch (target.type)
                {
                    case ItemType.acid:
                        // Do Nothing
                        break;

                    case ItemType.milk:
                        this.type = ItemType.yogurt;
                        isSuccess = true;
                        break;

                    case ItemType.oil:
                        this.type = ItemType.poison;
                        isSuccess = true;
                        break;
                }
            }
            else if (this.type == ItemType.oil)
            {
                switch (target.type)
                {
                    case ItemType.acid:
                        this.type = ItemType.poison;
                        isSuccess = true;
                        break;

                    case ItemType.milk:
                        this.type = ItemType.butter;
                        isSuccess = true;
                        break;

                    case ItemType.oil:
                        // Do Nothing
                        break;
                }
            }
            else
            {
                Debug.LogError("Error occues when fusing with type: " + this.type + " and " + target.type);
            }
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount: " + this.amount + " and " + target.amount);
        }

        return isSuccess;
    }

    public bool DeFuse(Item dropItem)
    {
        bool isSuccess = false;

        if (this.canFuse && this.amount >= 0 && dropItem.amount >= 0)
        {
            if (this.type == ItemType.yogurt)
            {
                switch (dropItem.type)
                {
                    case ItemType.acid:
                        this.type = ItemType.milk;
                        isSuccess = true;
                        break;

                    case ItemType.milk:
                        this.type = ItemType.acid;
                        isSuccess = true;
                        break;

                    case ItemType.oil:
                        // Do Nothing
                        break;
                }

            }
            else if (this.type == ItemType.butter)
            {
                switch (dropItem.type)
                {
                    case ItemType.acid:
                        // Do Nothing
                        break;

                    case ItemType.milk:
                        this.type = ItemType.oil;
                        isSuccess = true;
                        break;

                    case ItemType.oil:
                        this.type = ItemType.milk;
                        isSuccess = true;
                        break;
                }
            }
            else if (this.type == ItemType.poison)
            {
                switch (dropItem.type)
                {
                    case ItemType.acid:
                        this.type = ItemType.oil;
                        isSuccess = true;
                        break;

                    case ItemType.milk:
                        // Do Nothing
                        break;

                    case ItemType.oil:
                        
                        this.type = ItemType.acid;
                        isSuccess = true;
                        break;
                }
            }
            else
            {
                Debug.LogError("Error occues when fusing with type: " + this.type + " and " + dropItem.type);
            }
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount: " + this.amount + " and " + dropItem.amount);
        }

        return isSuccess;
    }

    ~Item()
    {

    }
}
