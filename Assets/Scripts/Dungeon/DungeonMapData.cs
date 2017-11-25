using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 一格地圖資料 </summary>
[System.Serializable]
public class DungeonMapData {
	/// <summary> 格子的位置 </summary>
	public Vector2 pos;
	/// <summary> 使用的地型種類 </summary>
	public E_DUNGEON_CUBE_TYPE cubeType;
	/// <summary> 使用的地型資料 </summary>
	public DungeonCubeData cubeData;
	/// <summary> 上面的道具種類 </summary>
	public Item.ItemType itemType;
	/// <summary> 上面的道具資料 </summary>
	public Item itemData;

	/// <summary> 地圖物件 </summary>
	public GameObject cubeObj;

	public DungeonMapData(Vector2 p_pos, int p_type, int p_itemType){
		Init (p_pos, (E_DUNGEON_CUBE_TYPE)p_type, (Item.ItemType)p_itemType);
	}
	public DungeonMapData(Vector2 p_pos, E_DUNGEON_CUBE_TYPE p_type, Item.ItemType p_itemType){
		Init (p_pos, p_type, p_itemType);
	}
	void Init(Vector2 p_pos, E_DUNGEON_CUBE_TYPE p_type, Item.ItemType p_itemType){
		pos = p_pos;
		cubeType = p_type;
		cubeData = DungeonManager.GetCubeData (p_type);

		itemType = p_itemType;
		itemData = ItemManager.GetItemData (p_type);
	}

}
