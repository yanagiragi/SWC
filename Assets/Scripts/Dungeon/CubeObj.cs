using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObj : MonoBehaviour {
	public MeshRenderer render;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColor (Color p_color) {
		render.material.SetColor ("_Color", p_color);
	}
}
