using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DUNGEON_CUBE_TYPE{
	HOME,
	NONE,
	WALL,
	EARTH,
	WATER,
	TRAP,
	LEN
}

/// <summary> 地型資料 </summary>
[System.Serializable]
public class DungeonCubeData{
	/// <summary> 地型物件的Prefab </summary>
	public GameObject cubePrefab;
	/// <summary> 地型種類 </summary>
	public E_DUNGEON_CUBE_TYPE type;
	/// <summary> 出現率 </summary>
	public float rate;
	/// <summary> 可以通過 </summary>
	public bool canThrough;
}
