using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager> {

    [ReorderableList]
    public List<int> PlayerItemList = new List<int>();

    [ReorderableList]
    public List<Material> MaterialMap = new List<Material>();

    public float health = 100;

	public float moveSpeed = 1;
	[LockInInspector]public Item.ItemType slimeMode = Item.ItemType.empty;
    public GameObject playerInstance;
    public GameObject yogurtInstance;
    public bool isIdle = true;

    private int yogurtCount = 0;

    private Vector3 nextPosition;
    private Vector3 rotationAngles;
    private Vector3 destination;

    public void Awake(){
        StepManager.step += PlayerManager.UpdateAtStep;
    }

	static public void ReStart(){
		instance.health = 100;

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
	}

    public void putYogurt()
    {
        yogurtInstance.transform.position = playerInstance.transform.position;
        yogurtCount = 0;
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
                GameOverManager.GameOver();
            }

            return;
        }

        bool isInteracted = CheckDropItemAction();
        if (isInteracted)
        {
            // Wait until Drop Menu Closed
            isIdle = false;

            UIManger.instance.OpenThrowUI();
        }
            
        if (isIdle)
        {
            GetNextStepTranslate();
        }
        
    }

    public void YogurtDisappear()
    {
        instance.yogurtCount = 0;
        yogurtInstance.transform.position = Vector3.right * -1f;
    }

    public static void IncreaseHealth(float amount)
    {
        if(instance.health < 100)
        {
            instance.health += amount;
        }
    }

    public static void DecreaseHealth(float amount)
    {
        if(instance.health > amount) { 
            instance.health -= amount;
        }
    }

    public bool CheckDropItemAction()
    {
        bool isInteracted = false;

        if (Input.GetKeyDown(KeyCode.K)) // Drop Item
        {
            isInteracted = true;
        }

        if (Input.GetKeyUp(KeyCode.K)) // Drop Item
        {
            isInteracted = true;
        }

        return isInteracted;
    }

	public void SetSlimeMode(Item.ItemType itemType)
	{
		slimeMode = itemType;
		Debug.Log ("SetSlimeMode : " + itemType.ToString());

        Material ReplaceMat = MaterialMap[(int) itemType];

        playerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material = ReplaceMat;
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
        if(instance.yogurtInstance.transform.position.x != -1)
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

    public void Move()
    {
        StartCoroutine(LerpPosition());

        bool condition = false;

        if (condition) {
            playerInstance.GetComponent<Animator>().Play("Armature|eat", -1, 0);
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
			AddItem (_data);
			DungeonManager.ChangeItemType ((int)destination.x, (int)destination.z, Item.ItemType.empty);
		}

		islerping = false;
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
            bool isConflictWithYogurt = (yogurtInstance.transform.position.x != -1 && (yogurtInstance.transform.position - _nextPos).magnitude < 0.01f);
			DungeonMapData _data = DungeonManager.GetMapData ((int)_nextPos.x, (int)_nextPos.z);

			if (_data.cubeData.canThrough && !isConflictWithYogurt) {
				if ((_data.itemData.canFuse) && (slimeMode != Item.ItemType.empty)) {
					Item.ItemType resultType = CheckFuse (slimeMode, _data.itemType);
					if ((int)resultType > (int)Item.ItemType.poison) {
						// 吃到全部，不能合成
					} else {
						StepManager.InvokeStep ();
					}
				}else{
					StepManager.InvokeStep ();
				}

			} else {
				Rotate ();
			}
        }
    }

	bool AddItem(DungeonMapData addItem)
    {
		Debug.Log ("AddItem : " + addItem.itemType.ToString());
        bool result = false;
		Item.ItemType _itemType = addItem.itemType;
		if (_itemType == Item.ItemType.food1 || _itemType == Item.ItemType.food2 || _itemType == Item.ItemType.food3)
        {
			++PlayerItemList[(int)_itemType];
            result = true;
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
