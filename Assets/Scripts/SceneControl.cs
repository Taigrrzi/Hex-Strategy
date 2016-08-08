using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Scene scene = SceneManager.GetActiveScene();
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
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
