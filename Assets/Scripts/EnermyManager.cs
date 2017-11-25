using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyManager : ManagerBase<EnermyManager> 
{
    public GameObject monster;
    public GameObject sulimo;
    public int mindist = 5;
    public float move = 1;


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
    }
}