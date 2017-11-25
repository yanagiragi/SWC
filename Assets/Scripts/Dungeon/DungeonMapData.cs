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
	public CubeObj cubeObj;

	public DungeonMapData(Vector2 p_pos, int p_type, int p_itemType){
		Init (p_pos, (E_DUNGEON_CUBE_TYPE)p_type, (Item.ItemType)p_itemType);
	}
	public DungeonMapData(Vector2 p_pos, E_DUNGEON_CUBE_TYPE p_type, Item.ItemType p_itemType){
		Init (p_pos, p_type, p_itemType);
	}
	void Init(Vector2 p_pos, E_DUNGEON_CUBE_TYPE p_type, Item.ItemType p_itemType){
		pos = p_pos;
		SetCubeType (p_type);
		SetCubeType (p_itemType);
	}

	public void SetCubeType(E_DUNGEON_CUBE_TYPE p_type){
		cubeType = p_type;
		cubeData = DungeonManager.GetCubeData (cubeType);
	}

	public void SetCubeType(Item.ItemType p_itemType){
		itemType = p_itemType;
		itemData = ItemManager.GetItemData (itemType);
	}

	#region"地圖生成"
	/// <summary> 區域ID </summary>
	public int groupID = -1;
	/// <summary> 相鄰區域ID </summary>
	public List<int> neighborGroupID = new List<int>();
	public int neighborCount = 0;
	public void AddNeighbor(int p_id){
		if (!neighborGroupID.Contains (p_id)) {
			neighborGroupID.Add (p_id);
			neighborCount++;
		}
	}
	public static int CompareByNeighborCount(DungeonMapData p_A, DungeonMapData p_B){
		return p_A.neighborCount.CompareTo (p_B.neighborCount);
	}
	#endregion
}
