using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 地城管理者 </summary>
public class DungeonManager : ManagerBase<DungeonManager> {
	const int sizeX = 100;
	const int sizeY = 100;

	/// <summary> 地城大小 </summary>
	public static Vector2 mapSize;

	/// <summary> 家位置 </summary>
	public static Vector2 homePos;

	/// <summary> 地形資料 </summary>
	[ReorderableList][SerializeField]
	DungeonCubeData[] cubeDatas;

//	/// <summary> 能在地城中出現的道具 </summary>
//	[SerializeField]
//	Item.ItemType[] itemsInDungeon;

	/// <summary> 地城格子種類 </summary>
	static DungeonMapData[,] mapsDatas = new DungeonMapData[sizeX, sizeY];

	public Transform dungeonTopObj;


#region "Init"
	/// <summary> 初始化地形資料 </summary>
	void InitCubeDatas () {
		int len = cubeDatas.Length;
		float _rateSum = 0;
		for (int f = 0; f < len; f++) {
			if (cubeDatas [f].rate >= 0) {
				_rateSum += cubeDatas [f].rate;
				cubeDatas [f].rate = _rateSum;
			}
		}
		for (int f = 0; f < len; f++) {
			if (cubeDatas [f].rate >= 0) {
				cubeDatas [f].rate /= _rateSum;
			}
		}

		List<Item> _itemList = ItemManager.instance.ItemList;
		len = _itemList.Count;
		_rateSum = 0;
		for (int f = 0; f < len; f++) {
			if (_itemList [f].rate >= 0) {
				_rateSum += _itemList [f].rate;
				_itemList [f].rate = _rateSum;
			}
		}
		for (int f = 0; f < len; f++) {
			if (_itemList [f].rate >= 0) {
				_itemList [f].rate /= _rateSum;
			}
		}
	}

	/// <summary> 初始化地城 </summary>
	void InitDungeon () {
		#region "mapsDatas"
		int x, y;
		mapSize = new Vector2 (sizeX, sizeY);
		homePos = new Vector2 (sizeX/2, sizeY/2);
		//隨機中央
		for (y = 1; y < (mapSize.y-1); y++) {
			for (x = 1; x < (mapSize.x-1); x++) {
				E_DUNGEON_CUBE_TYPE _cubeType = GetRandomCubeType();
				Item.ItemType _itemType = Item.ItemType.empty;
				if (_cubeType == E_DUNGEON_CUBE_TYPE.NONE) {
					_itemType = GetRandomItemType();
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

		//家
		Vector2[] _homePos = new Vector2[]{
			new Vector2(homePos.x - 1, homePos.y + 2),
			new Vector2(homePos.x    , homePos.y + 2),
			new Vector2(homePos.x + 1, homePos.y + 2),

			new Vector2(homePos.x - 2, homePos.y + 1),
			new Vector2(homePos.x - 1, homePos.y + 1),
			new Vector2(homePos.x    , homePos.y + 1),
			new Vector2(homePos.x + 1, homePos.y + 1),
			new Vector2(homePos.x + 2, homePos.y + 1),

			new Vector2(homePos.x - 2, homePos.y    ),
			new Vector2(homePos.x - 1, homePos.y    ),
			new Vector2(homePos.x    , homePos.y    ),
			new Vector2(homePos.x + 1, homePos.y    ),
			new Vector2(homePos.x + 2, homePos.y    ),

			new Vector2(homePos.x - 2, homePos.y - 1),
			new Vector2(homePos.x - 1, homePos.y - 1),
			new Vector2(homePos.x    , homePos.y - 1),
			new Vector2(homePos.x + 1, homePos.y - 1),
			new Vector2(homePos.x + 2, homePos.y - 1),

			new Vector2(homePos.x - 1, homePos.y - 2),
			new Vector2(homePos.x    , homePos.y - 2),
			new Vector2(homePos.x + 1, homePos.y - 2),
		};
		int len = _homePos.Length;
		for (int f = 0; f < len; f++) {
			Vector2 _pos = _homePos[f];
			mapsDatas [(int)_pos.x, (int)_pos.y] = new DungeonMapData(_pos,E_DUNGEON_CUBE_TYPE.HOME, Item.ItemType.empty);
		}
		#endregion

		#region "Road"
		//分組
		int _groupID = 0;
		List<DungeonMapData> _wallList = new List<DungeonMapData>();

		int _stackCount = 0;
		for (y = 1; y < (mapSize.y-1); y++) {
			for (x = 1; x < (mapSize.x-1); x++) {
				DungeonMapData _mapData = mapsDatas [x, y];
				if(_mapData.cubeData.canThrough){
					if( _mapData.groupID<0){
						_groupID++;

						PaintGroup(_mapData, _groupID);
					}
				}else{
					_wallList.Add(_mapData);
				}
			}
		}

//		dungeonTopObj = new GameObject().transform;
//		dungeonTopObj.gameObject.name = "First : ";
//		dungeonTopObj.gameObject.SetActive(false);
//		GenerateObj (_groupID);

		//建路
		List<int> _needRoadList = new List<int>();
		for (int f = 0; f <= _groupID; f++) {
			_needRoadList.Add(f);
		}

		int whileCount = 0;
		bool _hasChange;
		while(_needRoadList.Count > 0){
			_hasChange = false;
			whileCount++;
			if(whileCount >= 10){
				break;
			}
			_wallList.Sort(DungeonMapData.CompareByNeighborCount);
			len = _wallList.Count;
			for (int f = 0; f < len; f++) {
				DungeonMapData _wallData = _wallList[f];
				int len2 = _needRoadList.Count;
				bool _isFirst = true;
				int _firstID = 0;
				for (int f2 = 0; f2 < len2; f2++) {
					int _nowID = _needRoadList[f2];
					if(_wallData.neighborGroupID.Contains(_nowID)){
						if(_isFirst){
							_firstID = _nowID;
							_isFirst = false;
						}else{
							_wallData.SetCubeType(E_DUNGEON_CUBE_TYPE.NONE);
							_wallData.groupID = _nowID;
							PaintGroup(_wallData, _firstID);
//							Debug.Log("whileCount:" + whileCount + " pos:" + _wallData.pos + " firstID:" + _firstID + " nowID:" + _nowID);
							_needRoadList.RemoveAt(f2);
							len2--;
							f2--;
							_hasChange = true;
							break;
						}
					}
				}
			}

//			dungeonTopObj = new GameObject().transform;
//			dungeonTopObj.gameObject.name = "whileCount : " + whileCount;
//			dungeonTopObj.gameObject.SetActive(false);
//			GenerateObj (_groupID);

			if(!_hasChange){
				break;
			}
		}
//		Debug.Log("whileCount : " + whileCount);
		#endregion

		#region "Block"

		int _mainGroupId = mapsDatas [sizeX/2, sizeY/2].groupID;

		for (y = 1; y < (mapSize.y-1); y++) {
			for (x = 1; x < (mapSize.x-1); x++) {
				DungeonMapData _mapData = mapsDatas [x, y];
				if(_mapData.cubeData.canThrough && (_mapData.groupID != _mainGroupId)){
					_mapData.SetCubeType(E_DUNGEON_CUBE_TYPE.WALL);
					_mapData.groupID = -1;
				}
			}
		}

		#endregion

//		GenerateObjWithColor (_groupID);
	}

	Color[] _colors = null;
	void GenerateObj(){
		for (int y = 0; y < mapSize.y; y++) {
			for (int x = 0; x < mapSize.x; x++) {
				mapsDatas [x, y].RefrashObj();
			}
		}
	}
	void GenerateObjWithColor(int p_maxGroupID){
		if (_colors == null) {
			_colors = new Color[p_maxGroupID + 1];
			for (int f = 0; f <= p_maxGroupID; f++) {
				_colors [f] = Random.ColorHSV ();
			}
		}
		for (int y = 0; y < mapSize.y; y++) {
			for (int x = 0; x < mapSize.x; x++) {
				DungeonMapData _data = mapsDatas [x, y];
				_data.cubeObj = Instantiate (_data.cubeData.cubePrefab).GetComponent<CubeObj>();
				_data.cubeObj.transform.position = new Vector3(_data.pos.x, 0, _data.pos.y);
				_data.cubeObj.transform.SetParent (dungeonTopObj);
				if(_data.groupID >= 0){
					_data.cubeObj.SetColor(_colors[_data.groupID]);
//				}else if(_data.groupID == -2){
//					_data.cubeObj.SetColor(Color.white);
				}else{
					_data.cubeObj.SetColor(Color.blue);
				}
			}
		}
	}
		
	Vector2[] neighborPos = new Vector2[]{
		new Vector2( 1,  0),
		new Vector2(-1,  0),
		new Vector2( 0,  1),
		new Vector2( 0, -1)
	};
	void PaintGroup(DungeonMapData p_mapData, int p_groupId){
		Stack<DungeonMapData> _mapStack = new Stack<DungeonMapData>();
		int _targetId = p_mapData.groupID;
		_mapStack.Push(p_mapData);
		int _stackCount = 1;

		while(_stackCount > 0){
			DungeonMapData _nowData = _mapStack.Pop();
			_stackCount--;
			_nowData.groupID = p_groupId;

			for(int f=0; f<4; f++){
				Vector2 _subPos = _nowData.pos + neighborPos[f];
				DungeonMapData _subData = mapsDatas [(int)_subPos.x, (int)_subPos.y];
				if(_subData.cubeData.canThrough){
					if(_subData.groupID == _targetId){
						_mapStack.Push(_subData);
						_stackCount++;
					}
				}else{
					_subData.AddNeighbor(p_groupId);
				}
			}
		}
	}

	E_DUNGEON_CUBE_TYPE GetRandomCubeType(){
		float _random = Random.value;
		int len = cubeDatas.Length;
		for (int f = 0; f < len; f++) {
			if (cubeDatas [f].rate >= 0) {
				if (_random <= cubeDatas [f].rate) {
					return cubeDatas [f].type;
				}
			}
		}
		return E_DUNGEON_CUBE_TYPE.NONE;
	}

	Item.ItemType GetRandomItemType(){
		List<Item> _itemList = ItemManager.instance.ItemList;
		float _random = Random.value;
		int len = _itemList.Count;
		for (int f = 0; f < len; f++) {
			if (_itemList [f].rate >= 0) {
				if (_random <= _itemList [f].rate) {
					return _itemList [f].type;
				}
			}
		}
		return Item.ItemType.empty;
	}
#endregion

#region "取資料"
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
#endregion

	/// <summary>
	/// 改變 p_pos 位置的地城格子種類
	/// </summary>
	static public void ChangeCubeType(Vector2 p_pos, E_DUNGEON_CUBE_TYPE p_type){
		ChangeCubeType ((int)p_pos.x, (int)p_pos.y, p_type);
	}

	/// <summary>
	/// 改變 p_x, p_y 位置的地城格子種類
	/// </summary>
	static public void ChangeCubeType(int p_x, int p_y, E_DUNGEON_CUBE_TYPE p_type){
		DungeonMapData _mapData = mapsDatas[p_x, p_y];
		_mapData.SetCubeType (p_type);
		_mapData.RefrashObj ();
	}

	/// <summary>
	/// 改變 p_pos 位置的道具種類
	/// </summary>
	static public void ChangeItemType(Vector2 p_pos, Item.ItemType p_type){
		ChangeItemType ((int)p_pos.x, (int)p_pos.y, p_type);
	}

	/// <summary>
	/// 改變 p_x, p_y 位置的道具種類
	/// </summary>
	static public void ChangeItemType(int p_x, int p_y, Item.ItemType p_type){
		DungeonMapData _mapData = mapsDatas[p_x, p_y];
		_mapData.SetItemType (p_type);
		_mapData.RefrashObj ();
	}

	[Button]public string changeTypeTestBut = "ChangeTypeTest";
	public void ChangeTypeTest(){
		for (int f = 0; f < 200; f++) {
			ChangeCubeType (Random.Range (0, 100), Random.Range (0, 100), GetRandomCubeType ());
			ChangeItemType (Random.Range (0, 100), Random.Range (0, 100), GetRandomItemType ());
		}
	}

	void Awake () {
		InitCubeDatas ();
	}
	static public void ReStart(){
		instance.InitDungeon ();
		instance.GenerateObj ();
	}
	void Start () {
	}

	void Update () {
//		ChangeTypeTest ();
	}
}
