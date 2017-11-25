using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 地城管理者 </summary>
public class DungeonManager : ManagerBase<DungeonManager> {
	const int sizeX = 100;
	const int sizeY = 100;

	/// <summary> 地城大小 </summary>
	static Vector2 mapSize;

	/// <summary> 地形資料 </summary>
	[ReorderableList][SerializeField]
	DungeonCubeData[] cubeDatas;

	/// <summary> 能在地城中出現的道具 </summary>
	[SerializeField]
	Item.ItemType[] itemsInDungeon;

	/// <summary> 地城格子種類 </summary>
	static DungeonMapData[,] mapsDatas = new DungeonMapData[sizeX, sizeY];

	public Transform dungeonTopObj;

	/// <summary> 初始化地城 </summary>
	void InitDungeon () {
		int x, y;
		mapSize = new Vector2 (sizeX, sizeY);
		//隨機中央
		int _itemCount = itemsInDungeon.Length;
		for (y = 1; y < (mapSize.y-1); y++) {
			for (x = 1; x < (mapSize.x-1); x++) {
				E_DUNGEON_CUBE_TYPE _cubeType = (E_DUNGEON_CUBE_TYPE)Random.Range (0, (int)E_DUNGEON_CUBE_TYPE.LEN);
				Item.ItemType _itemType = Item.ItemType.empty;
				if (_cubeType == E_DUNGEON_CUBE_TYPE.NONE) {
					_itemType = itemsInDungeon[Random.Range (0, _itemCount)];
				}
				mapsDatas [x, y] = new DungeonMapData(new Vector2(x, y), _cubeType, _itemType);
			}
		}
		//上下外牆
		y = (int)mapSize.y - 1;
		for (x = 0; x < mapSize.x; x++) {
			mapsDatas [x, 0] = new DungeonMapData(new Vector2(x, 0),E_DUNGEON_CUBE_TYPE.WALL, Item.ItemType.empty);
			mapsDatas [x, y] = new DungeonMapData(new Vector2(x, y),E_DUNGEON_CUBE_TYPE.WALL, Item.ItemType.empty);
		}
		x = (int)mapSize.x - 1;
		//左右外牆
		for (y = 1; y < (mapSize.y-1); y++) {
			mapsDatas [0, y] = new DungeonMapData(new Vector2(0, y),E_DUNGEON_CUBE_TYPE.WALL, Item.ItemType.empty);
			mapsDatas [x, y] = new DungeonMapData(new Vector2(x, y),E_DUNGEON_CUBE_TYPE.WALL, Item.ItemType.empty);
		}

		for (y = 0; y < mapSize.y; y++) {
			for (x = 0; x < mapSize.x; x++) {
				DungeonMapData _data = mapsDatas [x, y];
				_data.cubeObj = Instantiate (_data.cubeData.cubePrefab);
				_data.cubeObj.transform.position = new Vector3(_data.pos.x, 0, _data.pos.y);
				_data.cubeObj.transform.SetParent (dungeonTopObj);
			}
		}
	}

	/// <summary>
	/// 取得 p_type 種類的地形資料
	/// </summary>
	static public DungeonCubeData GetCubeData(E_DUNGEON_CUBE_TYPE p_type){
		return GetCubeData ((int)p_type);
	}

	/// <summary>
	/// 取得第 p_index 個地形資料
	/// </summary>
	static public DungeonCubeData GetCubeData(int p_index){
		return instance.cubeDatas[p_index];
	}

	/// <summary>
	/// 取得 p_pos 位置的地城格子資料
	/// </summary>
	static public DungeonMapData GetMapData(Vector2 p_pos){
		return GetMapData ((int)p_pos.x, (int)p_pos.y);
	}

	/// <summary>
	/// 取得 p_x, p_y 位置的地城格子資料
	/// </summary>
	static public DungeonMapData GetMapData(int p_x, int p_y){
		return mapsDatas[p_x, p_y];
	}

	void Awake () {
		InitDungeon ();
	}

	void Start () {
		
	}

	void Update () {
		
	}
}
