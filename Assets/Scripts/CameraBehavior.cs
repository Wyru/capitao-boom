using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    private Texture2D fadeTexture;
    private float fadeSpeed = 0.2f;
    private int drawDepth = -1000;

    private float alpha = 1.0f;
    private float fadeDir = -1f;

    public GameObject player;

    private Vector3 offset;         

    // Use this for initialization
    void Start() {
        
        offset = transform.position - player.transform.position;
    }

    
    void LateUpdate() {
        
        transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);
    }

   /* public OnGUI()
    {

        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color.a = alpha;

        GUI.depth = drawDepth;

        GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    } */
}
