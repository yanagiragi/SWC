using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : ManagerBase<NPCManager> {

    [ReorderableList]
    public List<GameObject> kids = new List<GameObject>();

	public void UpdateAtStep () {
	    for(int i = 0; i < instance.kids.Count; ++i)
        {
            float rand = Random.Range(0, 100);
            int animIndex = 0;

            if(rand > 80)
            {
                animIndex = 1;
            }
            else if(rand > 60)
            {
                animIndex = 2;
            }
            
            if(animIndex > 0)
            {
                instance.kids[i].GetComponent<Animator>().SetInteger("AnimIndex", animIndex);
            }

            kids[i].transform.LookAt(PlayerManager.instance.playerInstance.transform);

        }
	}
	
	
}
