using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : ManagerBase<EnemyBehavior>
{
    public GameObject monsterBasePrefab;
    
    [ReorderableList]
    public List<Enemy> enemyList = new List<Enemy>();

    [ReorderableList]
    public List<int> randomIndex = new List<int>();

    private void Awake()
    {
        StepManager.step += EnemyBehavior.instance.UpdateAtStep;

        for (int i = 0; i < 4; ++i)
        {
            randomIndex.Add(i);
        }        
    }

    void Start()
    {
        enemyList.Clear();

        for (int i = 0; i < 1; i++)
        {
            GameObject m = Instantiate(monsterBasePrefab, GetEmptyPos(), transform.rotation);
            enemyList.Add(new Enemy(m, 0));
        }
    }

    Vector3 GetEmptyPos()
    {
        // Try 30 times, may need refactor
        for (int j = 1; j < 30; j++)
        {
            float x = Mathf.Floor(UnityEngine.Random.value * DungeonManager.mapSize.x);
            float y = Mathf.Floor(UnityEngine.Random.value * DungeonManager.mapSize.y);

            Vector2 _pos = new Vector2(x, y);
            DungeonMapData _data = DungeonManager.GetMapData(_pos);
            E_DUNGEON_CUBE_TYPE _type = _data.cubeType;
            if (_type == E_DUNGEON_CUBE_TYPE.NONE)
            {
                return new Vector3(_pos.x, 0.5f, _pos.y);
            }
        }
        return Vector3.zero;
    }

    void Move(GameObject g, int incrementX, int incrementZ)
    {
        g.transform.position += new Vector3(incrementX, 0, incrementZ);
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
            bool isContact = checkContactYogurt(enemy);

            if (isContact && enemy.EatYogurtCount <= 5)
            {
                Debug.Log("Eat Yougurt!");
                ++ enemy.EatYogurtCount;

                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // Call Yogurt Delete
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                return;
            }
            else if(enemy.EatYogurtCount == 6)
            {
                // Free to go, let go of yogurt's trap!
                enemy.EatYogurtCount = 0;
            }

            Vector3 playerDirection = PlayerManager.instance.playerInstance.transform.position - enemy.monster.transform.position;
            Vector3 yogurtDirection = PlayerManager.instance.yogurtInstance.transform.position - enemy.monster.transform.position;

            Vector3 ActualDirection;

            if(yogurtDirection.x != -1)
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

            Debug.Log(System.String.Format("({0},{1})", x, z));
            Move(enemy.monster, x, z);
        }
    }
}