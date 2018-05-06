using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeText : MonoBehaviour {
	public Sprite[] sprites;
	public int number;
	public void change ()
	{		
		var myRenderer = GetComponent<SpriteRenderer> ();
		if (sprites.Length > 0 && number < sprites.Length-1) {			
			number++;
			myRenderer.sprite = sprites [number];
		} else if (number >= sprites.Length-1) {
			SceneManager.LoadScene("scene01", LoadSceneMode.Single);
		}
	}
	// Use this for initialization
	void Start () {	
		number = 0;
	}

	// Update is called once per frame
	void Update () {	
		if (Input.anyKeyDown && !Input.GetButtonDown("Fire1"))
			change();	
	}
}
