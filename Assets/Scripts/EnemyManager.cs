using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public GameObject monster;
    public int EatYogurtCount = 0;
    
    public Enemy(GameObject mon, int yogurtCount)
    {
        monster = mon;
        EatYogurtCount = yogurtCount;
    }
}
