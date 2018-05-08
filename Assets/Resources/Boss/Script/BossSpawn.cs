using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

    public Character player;
    public GameObject boss;
    public AudioSource bossAudioSource;
    public AudioClip bossTrack;

	// Use this for initialization
	void Start () {
        boss = Resources.Load("Boss/Boss", typeof(GameObject)) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if (player.canSpawnBoss == true)
        {
            bossAudioSource.Stop();
            bossAudioSource.clip = bossTrack;
            bossAudioSource.Play();
            Instantiate(boss, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
	}
}
