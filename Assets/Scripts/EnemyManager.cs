using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerBase<EnemyManager>
{
    public GameObject monster;
    public GameObject sulimo;
    public int mindist = 1;
    public float move = 1;
    public Animator attack,monstermove;//怪物移動攻擊動畫
    private int monsterHP;
    private bool priority=false;


    void Update()//怪物面向史萊姆 離開一定距離(mindist)去追蹤目標物件
    {

        if (priority == false)
        {
            Quaternion monsterRotation = Quaternion.LookRotation(sulimo.transform.position - monster.transform.position, Vector3.up);//
            float monsterRotateSpeed = 4;
            monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, monsterRotation, Time.deltaTime * monsterRotateSpeed);
            monster.transform.rotation = monsterRotation;
            if (Vector3.Distance(sulimo.transform.position, monster.transform.position) > mindist)
            {
                monstermove.Play(1);

                transform.position += transform.forward * move * Time.deltaTime;


            }
            else
            {
                attack.Play(1);
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