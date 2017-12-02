using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject BGM;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(BGM);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
            EnterGame();
        }
	}

	public void EnterGame()
	{
        //SceneManager.LoadScene("Main");
        SceneManager.LoadSceneAsync("Main");
    }

	public void ExitGame()
	{
        Application.Quit();
    }
}
