using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour {

	public float minY;
	public float timeToExplode;
	public Character playerStatus;
	private RaycastHit2D[] foesHit;
    private RaycastHit2D[] bossHit;
    public GameObject booom;

	// Use this for initialization
	void Start () {
        StartCoroutine("Timer");
	}

	void Update () {
        bool stopped = false;
        if (!stopped && this.transform.position.y < this.minY)
        {
            this.GetComponent<Rigidbody2D> ().gravityScale = 0;
            this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
            stopped = true;
        }
        CheckBoss();
        
    }

    private void CheckBoss ()
    {
        bool hitBoss = false;
        bossHit = Physics2D.CircleCastAll(this.transform.position, 0.1f, new Vector2(1, 1), 0.1f);
        for (int i = 0; i < bossHit.Length; ++i)
        {
            Debug.Log("Ave Markus!");
            if (bossHit[i].collider.tag == "Boss")
            {
                hitBoss = true;
            }
            if (hitBoss)
                this.Explode();
        }
    }

    IEnumerator Timer() {
        for (float i = 0; i < timeToExplode; i+=.1f) {
            yield return new WaitForSeconds(.1f);
        }
		Explode ();
    }

    private void Explode() {
        //chama a animação de explosão
        //raycast nos inimigos ao redor que estão na mesma camada
        //chama a função de dano neles
        Instantiate(booom,this.transform.position, Quaternion.identity);
		foesHit = Physics2D.CircleCastAll (this.transform.position, 1.0f, new Vector2(1,1), 1.0f);
		for (int i = 0; i < foesHit.Length; ++i) {
			if (foesHit [i].collider.tag == "Foe") {
				foesHit [i].collider.gameObject.GetComponent<Foe> ().Damage (1);
                playerStatus.boomPower++;
			}
            else if (foesHit[i].collider.tag == "Boss")
            {
                foesHit[i].collider.gameObject.GetComponent<BossBehavior>().dealDamage(1);
                playerStatus.boomPower++;
            }
		}
		playerStatus.bombsLeft++;

        Destroy(this.gameObject);
    }
}
