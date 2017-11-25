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
	/// <summary> 道具物件的Prefab </summary>
	public GameObject objPrefab;
	/// <summary> 出現率 </summary>
	public float rate;
	/// <summary> Ui 縮圖 </summary>
	public Sprite icon;
	/// <summary> 增加飽食 </summary>
	public float satiation;

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
