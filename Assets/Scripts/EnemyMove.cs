using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerBase<EnemyManager>
{
    public GameObject monster;
    public GameObject sulimo;
    public int mindist = 1;
    public float move = 1;
    public Animator attack;//怪物攻擊動畫
    private int monsterHP;


    void Update()//怪物面向史萊姆 離開一定距離(mindist)去追蹤目標物件
    {


        Quaternion monsterRotation = Quaternion.LookRotation(sulimo.transform.position - monster.transform.position, Vector3.up);//
        float monsterRotateSpeed = 4;
        monster.transform.rotation = Quaternion.Slerp(monster.transform.rotation, monsterRotation, Time.deltaTime * monsterRotateSpeed);
        monster.transform.rotation = monsterRotation;
        if (Vector3.Distance(sulimo.transform.position, monster.transform.position) > mindist)
        {

            transform.position += transform.forward * move * Time.deltaTime;


        }
        else
        {
            attack.Play(1);
        }
    }

}