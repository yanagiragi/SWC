using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : ManagerBase<EnemyBehavior>
{
    public GameObject monster;
    public GameObject sulimo;
    public int mindist = 1;
    public float move = 1;
    public Animator attack, monstermove;//怪物移動攻擊動畫
    private int monsterHP;
    private bool priority = false;

    [ReorderableList]
    public List<GameObject> enemyList = new List<GameObject>();

    private void Awake()
    {
        StepManager.step += EnemyBehavior.instance.UpdateAtStep;
    }

    void Move(GameObject g, int incrementX, int incrementY)
    {
        g.transform.position += new Vector3(incrementX, incrementY, 0);
    }

    Vector2 WalkDefault(Vector3 position)
    {
        Vector2 increment = Vector2.zero;
        Vector2 testPosition = Vector2.zero;
        Vector2 positionV2 = new Vector2(position.x, position.y);

        int i = 0;

        for (i = 0; i < 4; ++i)
        {
            switch (i)
            {
                case 0: // (1, 0)
                    increment.x = 0;
                    increment.y = 1;
                    testPosition = positionV2 + increment;
                    break;
                case 1: // (-1, 0)
                    increment.x = 0;
                    increment.y = 1;
                    testPosition = positionV2 + increment;
                    break;
                case 2: // (0, 1)
                    increment.x = 0;
                    increment.y = 1;
                    testPosition = positionV2 + increment;
                    break;
                case 3: // (0, -1)
                    increment.x = 0;
                    increment.y = 1;
                    testPosition = positionV2 + increment;
                    break;
            }

            if (DungeonManager.GetMapData(testPosition).cubeType == E_DUNGEON_CUBE_TYPE.NONE)
            {
                break;
            }
        }
        
        if(i == 4) // No Place to Go
        {
            increment = Vector2.zero;
        }
        
        return increment;
    }

    public void UpdateAtStep()//怪物面向史萊姆 離開一定距離(mindist)去追蹤目標物件
    {
        foreach(GameObject enemy in enemyList)
        {
            Vector3 playerDirection = PlayerManager.instance.playerInstance.transform.position - enemy.transform.position;
            Vector3 yogurtDirection = PlayerManager.instance.yogurtInstance.transform.position - enemy.transform.position;

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

            int x = (int)ActualDirection.x;
            int y = (int)ActualDirection.y;

            if(x == 0 && y == 0)
            {
                Vector2 Dir = WalkDefault(enemy.transform.position);
                x = (int)Dir.x;
                y = (int)Dir.y;
            }

            Move(enemy, x, y);
        }

        if (priority == false)
        {
            Quaternion monsterRotation = Quaternion.LookRotation(sulimo.transform.position - monster.transform.position, Vector3.up);//
            float monsterRotateSpeed = 4;
            monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, monsterRotation, Time.deltaTime * monsterRotateSpeed);
            monster.transform.rotation = monsterRotation;
            if (Vector3.Distance(sulimo.transform.position, monster.transform.position) > mindist)
            {
                //monstermove.Play(1);

                transform.position += transform.forward * move * Time.deltaTime;


            }
            else
            {
                //attack.Play(1);
            }
        }
    }
    void OnTriggerStay(Collider attract)//遇到優格被吸引 切斷攻擊以及移動
    {
        priority = true;
        if (priority = true)
        {

            if (attract.tag == "yogurt")
            {
                
            }

        }
    }
    void OnTriggerEnter(Collider Corrosion)//碰到毒 怪物掛點
    {
        if (Corrosion.tag == "poison")
        {
            Destroy(monster);
        }
    }

}