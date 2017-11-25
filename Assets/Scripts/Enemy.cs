using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public GameObject monster;
    public int EatYogurtCount = 0;
    public bool isDead = false;
    
    public Enemy(GameObject mon)
    {
        monster = mon;
        EatYogurtCount = 0;
        isDead = false;
    }

    public IEnumerator playDeadAnim()
    {
        monster.GetComponent<Animator>().SetTrigger("isDead");
        yield return new WaitForSeconds(1.5f);
        
        for(int i = 0; i < 30; ++i)
        {
            monster.transform.position += Vector3.down * 0.01f;
            yield return new WaitForEndOfFrame();
        }

        GameObject.Destroy(monster);
    }
}
