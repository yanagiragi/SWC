using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager> {

	[ReorderableList]
	public List<int> PlayerItemList = new List<int>();

	[ReorderableList]
	public List<Material> MaterialMap = new List<Material>();

	public float health = 100;
	public float food = 0;
	public float satiation = 1000;
	public float MonsterMinusHealth = 5;

	public float moveSpeed = 1;
	[LockInInspector]public Item.ItemType slimeMode = Item.ItemType.empty;
	[LockInInspector]public Item.ItemType subMode1 = Item.ItemType.empty;
	[LockInInspector]public Item.ItemType subMode2 = Item.ItemType.empty;
	public GameObject playerInstance;
	public GameObject yogurtInstance;
	public bool isIdle = true;

	private int yogurtCount = 0;
	private Vector3 lastFramePos;
	private Enemy LoserEnemy;

	private Vector3 nextPosition;
	private Vector3 rotationAngles;
	public Vector3 destination;

	public void Awake(){
		//StepManager.step += PlayerManager.UpdateAtStep;
	}

	static public void ReStart(){

		// enum order: empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3

		// Init PlayerItemList
		instance.PlayerItemList.Clear();
		for (int i = 0; i < 10; ++i)
		{
			instance.PlayerItemList.Add(0);
		}

		instance.destination = new Vector3 (DungeonManager.homePos.x, 0, DungeonManager.homePos.y);
		instance.playerInstance.transform.position = instance.destination;
		instance.isIdle = true;

		instance.SetSlimeMode (Item.ItemType.empty);
		SetHealth (100);
		SetFood (0);
		UIManger.instance.UpdateSatiation ();

		instance.playerInstance.GetComponent<Animator> ().Play ("Idle");
		GameOverManager.isGameOver = false;
	}

	public void putYogurt()
	{
		if (PlayerItemList[(int)Item.ItemType.yogurt] > 0)
		{
			PlayerItemList[(int)Item.ItemType.yogurt] = 0;
			yogurtInstance.transform.position = playerInstance.transform.position;
			yogurtCount = 0;
			SetSlimeMode(Item.ItemType.empty);
		}
		else
		{
			Debug.LogWarning("Attempt to drop yogurt with no yogurt at all.");
		}
	}

	public void YogurtDisappear()
	{
		instance.yogurtCount = 0;
		yogurtInstance.transform.position = Vector3.right * -9999f;
	}

	// Called Every Frame
	private void Update()
	{
		if (health <= 0)
		{
			if (!GameOverManager.isGameOver)
			{
				playerInstance.GetComponent<Animator>().SetTrigger("isDead");

				GameOverManager.isGameOver = true;
				UIManger.StartGameOver ();
			}

			return;
		}



		if (isIdle) {
			GetNextStepTranslate ();
		}

		CheckDropItemAction();

	}


	public static void SetHealth(float p_health)
	{
		instance.health = p_health;
		UIManger.instance.UpdateBloodBar ();
	}

	public static void IncreaseHealth(float amount)
	{
		instance.health = Mathf.Min(instance.health + amount, 100);
		UIManger.instance.UpdateBloodBar ();
	}

	public static void DecreaseHealth(float amount)
	{
		instance.health = Mathf.Max(instance.health - amount, 0);
		UIManger.instance.UpdateBloodBar ();
		UIManger.instance.PlayHurtEffect();
	}

	public static void SetFood(float p_food)
	{
		instance.food = p_food;
		UIManger.instance.UpdateFoodInfo ();
	}

	public static void IncreaseFood(float amount)
	{
		instance.food = Mathf.Max(instance.food + amount, 0);
		UIManger.instance.UpdateFoodInfo ();
	}

	public static void SetSatiation(float p_satiation)
	{
		instance.satiation = p_satiation;
		UIManger.instance.UpdateSatiation ();
	}

	public static void IncreaseSatiation(float amount)
	{
        instance.satiation = Mathf.Clamp(instance.satiation + amount, 0, 1000);
		UIManger.instance.UpdateSatiation ();
    }

	public void CheckDropItemAction()
	{
		Item _item = ItemManager.GetItemData (slimeMode);

		switch (_item.fuseLevel) {
		case 1:
			if (Input.GetKeyDown (KeyCode.K)) {
				PlayerManager.instance.PlayerItemList[(int)slimeMode] = 0;
				SetSlimeMode (Item.ItemType.empty);
			}

			break;
		case 2:
			if (Input.GetKeyDown (KeyCode.K)) {
				isIdle = false;
				UIManger.instance.OpenThrowUI ();
			}

			if (Input.GetKeyUp (KeyCode.K)) {
				isIdle = true;
				UIManger.instance.CloseThrowUI ();
			}

			if (UIManger.isThrowUIOpen) {
				if (Input.GetKeyDown (KeyCode.A)) {
					DeFuse (slimeMode, subMode1);
					isIdle = true;
					UIManger.instance.CloseThrowUI ();
				}

				if (Input.GetKeyDown (KeyCode.D)) {
					DeFuse (slimeMode, subMode2);
					isIdle = true;
					UIManger.instance.CloseThrowUI ();
				}
			}
			break;
		}
	}

	public void SetSlimeMode(Item.ItemType itemType)
	{
		slimeMode = itemType;
		Debug.Log ("SetSlimeMode : " + itemType.ToString());

		Item _item =  ItemManager.GetItemData (slimeMode);

		switch (slimeMode) {
		case Item.ItemType.butter:
			subMode1 = Item.ItemType.milk;
			subMode2 = Item.ItemType.oil;
			break;
		case Item.ItemType.yogurt:
			subMode1 = Item.ItemType.milk;
			subMode2 = Item.ItemType.acid;
			break;
		case Item.ItemType.poison:
			subMode1 = Item.ItemType.oil;
			subMode2 = Item.ItemType.acid;
			break;
		default:
			subMode1 = Item.ItemType.empty;
			subMode2 = Item.ItemType.empty;
			break;
		}

		Material ReplaceMat = MaterialMap[(int) itemType];

		playerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material = ReplaceMat;

		UIManger.instance.UpdateSlimeMode ();
	}

	public Item.ItemType CheckFuse(Item.ItemType itemType1, Item.ItemType itemType2)
	{
		bool isSuccess = false;

		bool item1CanFuse = ItemManager.instance.ItemList[(int)itemType1].canFuse;
		bool item2CanFuse = ItemManager.instance.ItemList[(int)itemType2].canFuse;

		Item.ItemType resultType = (Item.ItemType)((int)itemType1 | (int)itemType2);
		if ((int)resultType > (int)Item.ItemType.poison) {
			Debug.LogWarning("Error occues when fusing : Over Range");
			return resultType;
		}

		bool item1AmmountIsZero = (PlayerItemList[(int)itemType1] <= 0);
		bool item2AmmountIsZero = (PlayerItemList[(int)itemType2] <= 0);
		if (item1AmmountIsZero || item2AmmountIsZero) {
			Debug.LogWarning("Error occues when fusing : Bad amount");
			return Item.ItemType.empty;
		}

		if (item1CanFuse && item2CanFuse)
		{
			if (itemType1 != itemType2)
			{
				if ((resultType != itemType1) && (resultType != itemType2)) {
					return resultType;
				} else {
					Debug.LogWarning("Error occues when fusing : Already Fuse");
					return resultType;
				}
			}
			else
			{
				Debug.LogWarning("Error occues when fusing : Same type");
				return itemType1;
			}
		}
		else
		{
			Debug.LogWarning("Error occues when fusing : Can't Fuse");
			return Item.ItemType.empty;
		}
	}

	public bool Fuse(Item.ItemType itemType1, Item.ItemType itemType2){
		Item.ItemType resultType = CheckFuse (itemType1, itemType2);
		if ((int)resultType > (int)Item.ItemType.poison) {
			return false;
		}else if (resultType != Item.ItemType.empty) {
			PlayerItemList [(int)resultType] = 1;
			PlayerItemList [(int)itemType1] = 0;
			PlayerItemList [(int)itemType2] = 0;
			SetSlimeMode (resultType);
			return true;
		}
		return false;
	}

	public bool DeFuse(Item.ItemType FusedItemType, Item.ItemType dropItemType)
	{
		bool isSuccess = false;

		if (PlayerItemList[(int)FusedItemType] >= 0 && PlayerItemList[(int)dropItemType] == 0)
		{
			Item.ItemType resultType = (Item.ItemType)((int)FusedItemType - (int)dropItemType);
			PlayerItemList[(int)resultType] = 1;
			PlayerItemList[(int)FusedItemType] = 0;
			PlayerItemList[(int)dropItemType] = 0;
			SetSlimeMode (resultType);
		}
		else
		{
			Debug.LogWarning("Error occues when fusing with amount");
		}

		return isSuccess;
	}

	public static void UpdateAtStep()
	{
		// Check if Yogurt should automatically disppear after few steps
		if(instance.yogurtInstance.transform.position.x != -9999)
		{
			if(instance.yogurtCount >= 5)
			{
				instance.yogurtCount = 0;
				instance.YogurtDisappear();
			}
			++instance.yogurtCount;
		}

		instance.destination = instance.destination + instance.nextPosition;

		SlimeBehaviourManger.instance.UpdatePlayerPosition(instance.destination);

		instance.Move();
		instance.Rotate();

	}

	public int checkConflict()
	{
		int isConflict = 0;

		foreach (Enemy enemy in EnemyBehavior.instance.enemyList)
		{
			float distance = (destination - enemy.monster.transform.position).magnitude;
			float distance2 = (destination - enemy.lastFramePos).magnitude + (enemy.monster.transform.position - lastFramePos).magnitude;
			if (distance < 0.01f)
			{
				isConflict = 1;
				LoserEnemy = enemy;
				break; // lazy check
			}
			if(distance2 < 0.01f)
			{
				isConflict = 2;
				LoserEnemy = enemy;
				break;// lazy check
			}
		}

		if (slimeMode == Item.ItemType.poison)
		{
			isConflict += 3;
		}

		return isConflict;
	}

	public void Move()
	{
		lastFramePos = playerInstance.transform.position;

		int condition = checkConflict();

        // Check Sys Text
        DungeonMapData data1 = DungeonManager.GetMapData(new Vector2(PlayerManager.instance.playerInstance.transform.position.x, PlayerManager.instance.playerInstance.transform.position.z));
        DungeonMapData data2 = DungeonManager.GetMapData(new Vector2(instance.destination.x, PlayerManager.instance.destination.z));

        if (data1.cubeType != E_DUNGEON_CUBE_TYPE.HOME && data2.cubeType == E_DUNGEON_CUBE_TYPE.HOME && !UIManger.isSystemTextOpen)
        {
            UIManger.instance.ShowSystemText();
        }

        if (condition == 1)
		{
			destination = playerInstance.transform.position;
			DecreaseHealth(MonsterMinusHealth);
		}
		else if (condition == 2)
		{
			// 剛剛好跟怪錯位，扣血但是可以移動
			DecreaseHealth(MonsterMinusHealth);
			StartCoroutine(LerpPosition());
		}
		else if(condition == 0 || condition == 3)
		{
			StartCoroutine(LerpPosition());
		}
		else // Posion 的狀況
		{
			EnemyBehavior.instance.Dead(LoserEnemy);
			destination = playerInstance.transform.position;
		}
	}

	bool islerping = false;
	IEnumerator LerpPosition()
	{
		islerping = true;
		while ((playerInstance.transform.position - destination).sqrMagnitude > 0.0001f)
		{
			playerInstance.transform.position = Vector3.MoveTowards(playerInstance.transform.position, destination, moveSpeed * Time.deltaTime);
			yield return null;
		}
		playerInstance.transform.position = destination;

		DungeonMapData _data = DungeonManager.GetMapData ((int)destination.x, (int)destination.z);

		if ((_data.itemType != Item.ItemType.empty)) {
			AddItem (_data.itemData);
			DungeonManager.ChangeItemType ((int)destination.x, (int)destination.z, Item.ItemType.empty);
		}

		islerping = false;
		if (_data.cubeType == E_DUNGEON_CUBE_TYPE.HOME) {
			IncreaseSatiation (instance.food);
			SetFood (0);

		} else {
			IncreaseSatiation (-1f);
		}
		//		Fuse (slimeMode, _data.itemType);
	}

	public void Rotate()
	{
		playerInstance.transform.localEulerAngles = rotationAngles;
		rotationAngles = Vector3.zero;
	}

	Queue<KeyCode> keyQueue = new Queue<KeyCode>();

	public void GetNextStepTranslate()
	{        
		if (Input.GetKeyDown(KeyCode.W)){
			keyQueue.Enqueue (KeyCode.W);
		}
		else if (Input.GetKeyDown(KeyCode.S)){
			keyQueue.Enqueue (KeyCode.S);
		}
		else if (Input.GetKeyDown(KeyCode.A)){
			keyQueue.Enqueue (KeyCode.A);
		}
		else if (Input.GetKeyDown(KeyCode.D)){
			keyQueue.Enqueue (KeyCode.D);
		}


		if (!islerping)
		{
			if (keyQueue.Count <= 0) {
				return;
			}

			KeyCode _nowKey = keyQueue.Dequeue ();

			switch(_nowKey){
			case KeyCode.W:          
				nextPosition = Vector3.forward;
				rotationAngles = Vector3.zero * 90f;
				break;
			case KeyCode.S:
				nextPosition = Vector3.back;
				rotationAngles = Vector3.down * 180f;
				break;
			case KeyCode.A:
				nextPosition = Vector3.left;
				rotationAngles = Vector3.down * 90f;
				break;
			case KeyCode.D:
				nextPosition = Vector3.right;
				rotationAngles = Vector3.up * 90f;
				break;
			}

			playerInstance.GetComponent<Animator>().Play("Armature|jump", -1, 0);

			Vector3 _nextPos = instance.destination + instance.nextPosition;
			bool isConflictWithYogurt = (yogurtInstance.transform.position.x != -9999 && (yogurtInstance.transform.position - _nextPos).magnitude < 0.01f);
			DungeonMapData _data = DungeonManager.GetMapData ((int)_nextPos.x, (int)_nextPos.z);

            if (_data.cubeData.canThrough && !isConflictWithYogurt)
            {
                if ((_data.itemData.canFuse) && (slimeMode != Item.ItemType.empty)) {
                    Item.ItemType resultType = CheckFuse(slimeMode, _data.itemType);
                    if ((int)resultType > (int)Item.ItemType.poison) {
                        // 吃到全部，不能合成
                    } else {
                        StepManager.InvokeStep();
                    }
                } else {
                    StepManager.InvokeStep();
                }

                if (_data.cubeType == E_DUNGEON_CUBE_TYPE.WATER)
                {
                    SlimeBehaviourManger.instance.GetNextStep(_data);

                    SlimeBehaviourManger.instance.WalkOnWater();
                }

                if (_data.cubeType == E_DUNGEON_CUBE_TYPE.TRAP)
                {
                    SlimeBehaviourManger.instance.GetNextStep(_data);

                    SlimeBehaviourManger.instance.WalkOnTrap();
                }

            } else if (_data.cubeType == E_DUNGEON_CUBE_TYPE.EARTH && slimeMode == Item.ItemType.acid)
            {
                SlimeBehaviourManger.instance.GetNextStep(_data);

                SlimeBehaviourManger.instance.AcidMeltWall();
				Rotate();

			}
			else
			{
				Rotate();
			}
		}
	}

	bool AddItem(Item addItem)
	{
		Debug.Log ("AddItem : " + addItem.type.ToString());
		playerInstance.GetComponent<Animator>().Play("Armature|eat", -1, 0);
		bool result = false;
		Item.ItemType _itemType = addItem.type;
		if (_itemType == Item.ItemType.food1 || _itemType == Item.ItemType.food2 || _itemType == Item.ItemType.food3)
		{
			++PlayerItemList[(int)_itemType];
			result = true;
			IncreaseFood (addItem.satiation);
		}
		else
		{
			if(PlayerItemList[(int)_itemType] == 0)
			{
				++PlayerItemList[(int)_itemType];
				result = true;
				if (slimeMode == Item.ItemType.empty) {
					SetSlimeMode (_itemType);

				} else {
					Fuse (slimeMode, _itemType);
				}
			}
		}

		return result;
	}

	bool DropItem(Item.ItemType dropItemType)
	{
		bool result = false;

		if (ItemManager.instance.ItemList[(int)dropItemType].canDrop && PlayerItemList[(int)dropItemType] > 0)
		{
			--PlayerItemList[(int)dropItemType];
			result = true;
		}

		return result;
	}

}
