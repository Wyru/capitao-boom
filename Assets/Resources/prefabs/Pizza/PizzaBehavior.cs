using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //som de comendo pizza
            //som de heal
            //heal
            collision.gameObject.GetComponent<Character>().Heal(1);
            Destroy(this.gameObject);
        }
    }
}
