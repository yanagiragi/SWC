using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager: ManagerBase<SoundManager> 
{

	public AudioClip water,slimestep,acidDestroy,swallow,teleport,fire;
	public AudioSource audiosource;
	private GameObject Slime;

	public void UpdateAtStep()
	{
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

	public void do_acidDestroy()//player action
	{
		audiosource.PlayOneShot (acidDestroy, 1f);
	}

	public void do_teleport()//player action
	{
		audiosource.PlayOneShot (teleport, 1f);
	}

	public void do_swallow()//monster action
	{
		audiosource.PlayOneShot (swallow, 1f);
	}
}
