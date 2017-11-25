using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : ManagerBase<GameOverManager> {

    public static bool isGameOver = false;

	public static void GameOver()
    {
        Debug.Log("GameOver!");

        // After All Done

        RestartManager.ReStart();
    }
}
