using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerBase<EnemyManager>
{
    public GameObject monster;
    public int i;
    Vector3 pos = new Vector3(Random.Range(-100f, 100f), 4.5f, 0f);

    void Start()
    {
        
        for (i = 0; i<=30; i++)
            {
                Instantiate(monster, pos, transform.rotation);







            }
    }



}