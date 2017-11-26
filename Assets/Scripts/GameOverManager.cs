using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : ManagerBase<GameOverManager> {

    public static bool isGameOver = false;

	public void GameOver()
    {
        Debug.Log("GameOver!");
		RestartManager.ReStart ();
    }

	public void ChangeMap()
	{
		Debug.Log("ChangeMap!");
		RestartManager.ChangeMap ();
	}
}
