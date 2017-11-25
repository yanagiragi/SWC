using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase<PlayerManager> {

    [ReorderableList]
    public List<int> PlayerItemList = new List<int>();

    public float health = 100;

    public GameObject playerInstance;
    public GameObject yogurtInstance;
    public bool isIdle = true;
    private bool isDrop = false;
    private bool isWalk = false;
    private bool isEat = false;

    private Vector3 nextPosition;
    private Vector3 rotationAngles;
    private Vector3 destination;

    public void Start()
    {
        StepManager.step += PlayerManager.UpdateAtStep;

        health = 100;

        // enum order: empty, milk, oil, butter, acid, yogurt, poison, food1, food2, food3

        // Init PlayerItemList
        for (int i = 0; i < 10; ++i)
        {
            PlayerItemList.Add(0);
        }
    }

    // Called Every Frame
    private void Update()
    {
        if (health <= 0)
        {
            ; // Preform Death Action
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
            isEat = false;
            GetNextStepTranslate();
        }
        
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
            isDrop = true;
            isInteracted = true;
        }

        if (Input.GetKeyUp(KeyCode.K)) // Drop Item
        {
            isDrop = true;
            isInteracted = true;
        }

        return isInteracted;
    }

    public bool Fuse(Item.ItemType itemType1, Item.ItemType itemType2)
    {
        bool isSuccess = false;

        bool item1CanFuse = ItemManager.instance.ItemList[(int)itemType1].canFuse;
        bool item2CanFuse = ItemManager.instance.ItemList[(int)itemType2].canFuse;
        bool item1AmmountOverZero = PlayerItemList[(int)itemType1] >= 0;
        bool item2AmmountOverZero = PlayerItemList[(int)itemType2] >= 0;

        if (item1CanFuse && item2CanFuse && item1AmmountOverZero && item2AmmountOverZero)
        {
            if (itemType1 != itemType2)
            {
                Item.ItemType resultType = (Item.ItemType)((int)itemType1 + (int)itemType2);
                PlayerItemList[(int)resultType] = 1;
                PlayerItemList[(int)itemType1] = 0;
                PlayerItemList[(int)itemType2] = 0;
                isSuccess = true;
            }
            else
            {
                Debug.LogError("Error occues when fusing with type");
            }
        }
        else
        {
            Debug.LogError("Error occues when fusing with amount");
        }

        return isSuccess;
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
            Debug.LogError("Error occues when fusing with amount");
        }

        return isSuccess;
    }

    public static void UpdateAtStep()
    {
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
            isEat = true;
            playerInstance.GetComponent<Animator>().Play("Armature|eat", -1, 0);
        }
    }

    IEnumerator LerpPosition()
    {
        while (playerInstance.transform.position != destination)
        {
            playerInstance.transform.position = Vector3.Lerp(playerInstance.transform.position, destination, Time.deltaTime);
            yield return null;
            if (!isWalk)
                isWalk = false;
        }
    }

    public void Rotate()
    {
        playerInstance.transform.localEulerAngles = rotationAngles;
        rotationAngles = Vector3.zero;
    }


    public void GetNextStepTranslate()
    {
        bool isPress = false;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
			Debug.Log ("gameObject:" + gameObject.GetInstanceID() + " this:" + this.GetInstanceID());
            isPress = true;
            nextPosition = Vector3.right;
            rotationAngles = Vector3.zero * 90f;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            isPress = true;
            nextPosition = Vector3.left;
            rotationAngles = Vector3.down * 180f;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            isPress = true;
            nextPosition = Vector3.forward;
            rotationAngles = Vector3.down * 90f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            isPress = true;
            nextPosition = Vector3.back;
            rotationAngles = Vector3.up * 90f;
        }

        if (isPress)
        {
            if (!isWalk)
            {
                isWalk = true;
                playerInstance.GetComponent<Animator>().Play("Armature|jump", -1, 0);

                // Update Step
                StepManager.InvokeStep();
            }
        }
        else
        {
            isWalk = false;
            playerInstance.GetComponent<Animator>().SetBool("isWalk", isWalk);
        }
    }

    bool AddItem(Item.ItemType AddItemType)
    {
        bool result = false;

        if (AddItemType == Item.ItemType.food1 || AddItemType == Item.ItemType.food2 || AddItemType == Item.ItemType.food3)
        {
            ++PlayerItemList[(int)AddItemType];
            result = true;
        }
        else
        {
            if(PlayerItemList[(int)AddItemType] == 0)
            {
                ++PlayerItemList[(int)AddItemType];
                result = true;
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
