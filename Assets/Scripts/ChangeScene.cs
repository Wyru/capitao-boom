using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public void GoToIntro(){
		SceneManager.LoadScene("intro", LoadSceneMode.Single);
	}
	public void GoToScene01(){
		SceneManager.LoadScene("scene01", LoadSceneMode.Single);
	}
    public void GoToMenu ()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);

    }
	public void exitGame(){
		Application.Quit();
	}
}
