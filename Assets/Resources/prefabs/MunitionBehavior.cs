using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionBehavior : MonoBehaviour {
	private Character player;

	private Rigidbody2D body;
	private Vector2 direction;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").GetComponent<Character>();
		body = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D[] hit = Physics2D.CircleCastAll (this.transform.position, 0.1f, body.velocity, 0.05f);
		for (int i = 0; i < hit.Length; ++i) {
			if (hit[i].collider.name == "Player") {
				player.takeDamage (1);
				Destroy (this.gameObject);
			}
		}
		if (body.velocity == Vector2.zero)
			Destroy (this.gameObject);
	}
}
