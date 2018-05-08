using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateBombBehavior : MonoBehaviour {

    public float minY;
    public float timeToExplode;
    public Character playerStatus;
    private RaycastHit2D[] foesHit;
    public GameObject booom;


    // Use this for initialization
    void Start() {
        StartCoroutine("Timer");
    }

    void Update() {
        bool stopped = false;
        if (!stopped && this.transform.position.y < this.minY + 1) {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            stopped = true;
        }
    }

    IEnumerator Timer() {
        for (float i = 0; i < timeToExplode; i += .1f) {
            yield return new WaitForSeconds(.1f);
        }
        Explode();
    }

    private void Explode() {
        //chama a animação de explosão
        //raycast nos inimigos ao redor que estão na mesma camada
        //chama a função de dano neles
        Instantiate(booom, this.transform.position, Quaternion.identity);
        foesHit = Physics2D.CircleCastAll(this.transform.position, 3.0f, new Vector2(1, 2), 3.0f);
        for (int i = 0; i < foesHit.Length; ++i) {
            if (foesHit[i].collider != null && foesHit[i].collider.name != "Player") {
                Debug.Log(foesHit[i].collider.name);
                foesHit[i].collider.gameObject.GetComponent<Foe>().Damage(5);
            }
        }
        Destroy(this.gameObject);
    }
}
