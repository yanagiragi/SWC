using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_PARTICLE_TYPE{WATER, HIT, ROCK, ADD}
public class ParticleManager : ManagerBase<ParticleManager> {
	public List<GameObject> particleList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// 在 p_pos 位置生成粒子
	/// </summary>
	static public void ShowParticle(Vector2 p_pos, E_PARTICLE_TYPE p_type){
		ShowParticle ((int)p_pos.x, (int)p_pos.y, p_type);
	}

	/// <summary>
	/// 在 p_x, p_y 位置生成粒子
	/// </summary>
	static public void ShowParticle(int p_x, int p_y, E_PARTICLE_TYPE p_type){
		GameObject _obj = Instantiate (instance.particleList[(int)p_type]);
		_obj.transform.position = new Vector3 (p_x, 0.1f, p_y);
	}
}
