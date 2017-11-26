using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Note: No Dealing Enemy Conflicts with each other
 */

public class EnemyBehavior : ManagerBase<EnemyBehavior>
{
    public GameObject monsterBasePrefab;
    
    [ReorderableList]
    public List<Enemy> enemyList = new List<Enemy>();

    [ReorderableList]
    public List<int> randomIndex = new List<int>();

    public int EnemyAmount;

    private void Awake()
    {
        for (int i = 0; i < 4; ++i)
        {
            randomIndex.Add(i);
        }        
    }

    public void Init()
    {
        enemyList.Clear();

        for (int i = 0; i < EnemyAmount; i++)
        {
            Vector3 pos = GetEmptyPos();

            // For Debug
            if(i == 0)
            {
                ;// pos = new Vector3(52, 0, 52);
            }

            GameObject m = Instantiate(monsterBasePrefab, pos , transform.rotation);
            enemyList.Add(new Enemy(m));
        }
    }

    Vector3 GetEmptyPos()
    {
        // Try 30 times, may need refactor
        for (;;)
        {
            float x = Mathf.Floor(UnityEngine.Random.Range(0, DungeonManager.mapSize.x - 1));
            float y = Mathf.Floor(UnityEngine.Random.Range(0, DungeonManager.mapSize.y - 1));

            Vector2 _pos = new Vector2(x, y);
            DungeonMapData _data = DungeonManager.GetMapData(_pos);
            E_DUNGEON_CUBE_TYPE _type = _data.cubeType;
            if (_type == E_DUNGEON_CUBE_TYPE.NONE)
            {
                return new Vector3(_pos.x, 0f, _pos.y);
            }            
        }
        return Vector3.zero;
    }

    void Move(Enemy e, int incrementX, int incrementZ)
    {
        Vector3 angles = Vector3.up;

        if (incrementX != 0)
        {
            if(incrementX > 0)
            {
                angles = Vector3.up;
            }
            else
            {
                angles = Vector3.down;
            }
        }
        else
        {
            if(incrementZ > 0)
            {
                angles = Vector3.zero;
            }
            else
            {
                angles = Vector3.down * 2;
            }
        }

        e.monster.transform.localEulerAngles = angles * 90f;

        e.monster.GetComponent<Animator>().Play("metarig|walk", -1, 0);

        e.lastFramePos = e.monster.transform.position;

        e.monster.transform.position = e.monster.transform.position + new Vector3(incrementX, 0, incrementZ);
    }

    Vector2 WalkDefault(Vector3 position)
    {
        Vector2 increment = Vector2.zero;
        Vector2 testPosition = Vector2.zero;
        Vector2 positionV2 = new Vector2(position.x, position.z);

        int i = 0;

        // i = 0 already init
        for (int offset = (int)Random.Range(0, 100); i < 4; ++i)
        {
            int index = (offset + i) % 4;
            int tmp = randomIndex[i];
            randomIndex[i] = randomIndex[index];
            randomIndex[index] = tmp;
        }

        for (i = 0; i < 4; ++i)
        {
            switch (randomIndex[i])
            {
                case 0: // (1, 0)
                    increment.x = 1;
                    increment.y = 0;
                    testPosition = positionV2 + increment;
                    break;
                case 1: // (0, -1)
                    increment.x = 0;
                    increment.y = -1;
                    testPosition = positionV2 + increment;
                    break;
                case 2: // (-1, 0)
                    increment.x = -1;
                    increment.y = 0;
                    testPosition = positionV2 + increment;
                    break;
                case 3: // (0, 1)
                    increment.x = 0;
                    increment.y = 1;
                    testPosition = positionV2 + increment;
                    break;
            }

            DungeonMapData cubeData;
            try
            {
                cubeData = DungeonManager.GetMapData(testPosition);
                if (cubeData.cubeData.canThrough)
                {
                    break;
                }
            }
            catch(System.Exception e)
            {
                ;
            }
        }
        
        if(i == 4) // No Place to Go
        {
            increment = Vector2.zero;
        }

        return increment;
    }

    public bool checkContactYogurt(Enemy enemy)
    {
        return (enemy.monster.transform.position - PlayerManager.instance.yogurtInstance.transform.position).sqrMagnitude < 0.01;
    }

    public void UpdateAtStep()//怪物面向史萊姆 離開一定距離(mindist)去追蹤目標物件
    {
        foreach(Enemy enemy in enemyList)
        {
            if (enemy.isDead)
            {
                continue;
            }

            bool isContact = checkContactYogurt(enemy);

            if (isContact && enemy.EatYogurtCount <= 15)
            {
                Debug.Log("Eat Yougurt!");
                ++ enemy.EatYogurtCount;

                PlayerManager.instance.YogurtDisappear();

                return;
            }
            else if(enemy.EatYogurtCount == 16)
            {
                // Free to go, let go of yogurt's trap!
                enemy.EatYogurtCount = 0;
            }

            Vector3 playerDirection = PlayerManager.instance.playerInstance.transform.position - enemy.monster.transform.position;
            Vector3 yogurtDirection = PlayerManager.instance.yogurtInstance.transform.position - enemy.monster.transform.position;

            Vector3 ActualDirection;

            if(yogurtDirection.x != -9999)
            {
                // Yogurt is Still Alive!
                ActualDirection = yogurtDirection.normalized;
            }
            else
            {
                ActualDirection = playerDirection.normalized;
            }

            int x = 0;
            int z = 0;
            
            float max = Mathf.Max(Mathf.Abs(ActualDirection.x), Mathf.Abs(ActualDirection.z));
            if(Mathf.Abs(ActualDirection.x) > Mathf.Abs(ActualDirection.z))
            {
                x = (int)Mathf.Sign(ActualDirection.x) *  Mathf.CeilToInt(Mathf.Abs(ActualDirection.x));
                z = 0;
            }
            else
            {
                z = (int)Mathf.Sign(ActualDirection.z) * Mathf.CeilToInt(Mathf.Abs(ActualDirection.z));
                x = 0;
            }

            //Debug.Log(System.String.Format("({0},{1})", x, z));

            if (x == 0 && z == 0)
            {
                Vector2 Dir = WalkDefault(enemy.monster.transform.position);
                x = (int)Dir.x;
                z = (int)Dir.y;
            }
            else
            {
                DungeonMapData cubeData;
                try
                {
                    Vector2 newPos =  new Vector2(enemy.monster.transform.position.x + x, enemy.monster.transform.position.z + z);
                    cubeData = DungeonManager.GetMapData(newPos);
                    if (cubeData.cubeData.canThrough)
                    {
                        ;
                    }
                    else
                    {
                        Vector2 Dir = WalkDefault(enemy.monster.transform.position);
                        x = (int)Dir.x;
                        z = (int)Dir.y;
                    }
                    
                }
                catch (System.Exception e)
                {
                    Vector2 Dir = WalkDefault(enemy.monster.transform.position);
                    x = (int)Dir.x;
                    z = (int)Dir.y;
                }
            }

            //Debug.Log(System.String.Format("({0},{1})", x, z));
            Move(enemy, x, z);
        }
    }
    
    public void Dead(Enemy deadEnemy)
    {
        enemyList.Remove(deadEnemy);

        deadEnemy.isDead = true;

        deadEnemy.monster.transform.LookAt(PlayerManager.instance.playerInstance.transform);

        StartCoroutine(deadEnemy.playDeadAnim());
    }
}