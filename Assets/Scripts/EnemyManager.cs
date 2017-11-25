using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerBase<EnemyManager>
{
    public GameObject monster;
    public int i,j=1;

  

    void Start()
    {
        
        for (i = 0; i<=30; i++)
            {
                Instantiate(monster, GetEmptyPos(), transform.rotation);







            }
    }


	Vector3 GetEmptyPos()
	{   

		for(j=1;j<30;j++)
		{
			float x = Mathf.Floor(Random.value*DungeonManager.mapSize.x);
			float y = Mathf.Floor(Random.value*DungeonManager.mapSize.y);


			Vector2 _pos = new Vector2(x,y);
			DungeonMapData _data = DungeonManager.GetMapData(_pos);
			E_DUNGEON_CUBE_TYPE _type = _data.cubeType;
			if( _type == E_DUNGEON_CUBE_TYPE.NONE )
			{

				return new Vector3(_pos.x, 0.5f, _pos.y);
			
			}

		}
		return Vector3.zero;
	}
}
