using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DUNGEON_CUBE_TYPE{
	NONE,
	WALL,
	WATER,
	LEN
}

/// <summary> 地型資料 </summary>
[System.Serializable]
public class DungeonCubeData{
	/// <summary> 地型物件的Prefab </summary>
	public GameObject cubePrefab;
	/// <summary> 地型種類 </summary>
	public E_DUNGEON_CUBE_TYPE type;



}
