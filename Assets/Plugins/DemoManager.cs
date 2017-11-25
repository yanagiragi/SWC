using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : ManagerBase<DemoManager> {
	int privateA;
	public static int A{
		get{
			return instance.privateA;
		}
	}
	public int B;

	public static void FunA(){

	}

	public void FunB(){

	}

	void Start () {
		//其他腳本使用：
		Debug.Log(DemoManager.A);
		Debug.Log(DemoManager.instance.B);
		DemoManager.FunA ();
		DemoManager.instance.FunB ();
	}
	void Update () {
		
	}
}
