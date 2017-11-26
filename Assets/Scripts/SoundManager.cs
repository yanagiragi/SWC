using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager: ManagerBase<SoundManager> 
{

	public AudioClip water,slimestep,acidDestroy,swallow,teleport,fire;
	AudioSource audiosource;
	public GameObject Slime;

	void Start()
	{
		audiosource = GetComponent<AudioSource> ();
	}

	public void UpdateAtStep()
	{
		Debug.Log("text");
		Vector3 _pos3 = PlayerManager.instance.playerInstance.transform.position;
		Vector2 _pos = new Vector2 (_pos3.x, _pos3.z);
		DungeonMapData _data = DungeonManager.GetMapData (_pos);
		E_DUNGEON_CUBE_TYPE _type = _data.cubeType;

		if(_type == E_DUNGEON_CUBE_TYPE.NONE)
		{
			audiosource.PlayOneShot(slimestep,1f);
		}
		if(_type == E_DUNGEON_CUBE_TYPE.WATER)
		{
			audiosource.PlayOneShot(water,1f);
		}
		if(_type == E_DUNGEON_CUBE_TYPE.TRAP)
		{
			audiosource.PlayOneShot(fire,1f);
		}


	}
	void do_acidDestroy()
	{
		
	}
	void do_swallow()
	{

	}
	void do_teleport()
	{

	}
}
