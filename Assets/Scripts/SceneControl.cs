using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneControl : MonoBehaviour {

    public List<int> Team0;
    public List<int> Team1;

	// Use this for initialization
	void Start () {
        //Scene scene = SceneManager.GetActiveScene();
        DontDestroyOnLoad(this);
	}

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Initial");
    }

    public void StartCollection()
    {
        SceneManager.LoadScene("Scenes/Collection");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
