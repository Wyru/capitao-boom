using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
	
	public Character player;

	public GameObject enemyA;
	public GameObject enemyB;
	public GameObject enemyC;

	public int instanceNo;

	public float spawnTime = 3f;

	private int totalSpawn;
	private int spawned;

	// Use this for initialization
	void Start () {
        spawned = 0;

        player = GameObject.FindWithTag ("Player").GetComponent<Character>();

		switch (instanceNo) {
		    case 1:
			    totalSpawn = 20;
			break;
		    case 2:
			    totalSpawn = 20;
			break;
            case 3:
                totalSpawn = 15;
            break;
            case 4:
                totalSpawn = 15;
            break;
            case 0:
                totalSpawn = 0;
            break;
		    default:
			    totalSpawn = 10;
			break;
		}
		
		enemyA = Resources.Load ("prefabs/foe/Types/Foe", typeof (GameObject)) as GameObject;
		enemyB = Resources.Load ("prefabs/foe/Types/Foe2", typeof (GameObject)) as GameObject;
        enemyC = Resources.Load ("prefabs/foe/Types/Foe3", typeof(GameObject)) as GameObject;

        if (spawned < totalSpawn) { InvokeRepeating("Spawn", spawnTime, spawnTime); }
	}

    void Update()
    {
        if (player.canSpawnBoss == true && this.instanceNo < 3)
        {
            Destroy(this.gameObject);
        } 
        else if (player.bossDead == true)
        {
            Destroy(this.gameObject);
        }
    }

    void Spawn () {
		float distance = Mathf.Abs (player.transform.position.x - this.transform.position.x);
        if (distance > 5.2f && distance < 25.0f) {
            if ((++spawned % 3) == 0) {
                Instantiate (enemyB, this.transform.position, Quaternion.identity);
            } else {  
				Instantiate (enemyA, this.transform.position, Quaternion.identity); 
			} 
		} 

    }
}
