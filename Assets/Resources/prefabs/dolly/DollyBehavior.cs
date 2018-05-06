using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyBehavior : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //som tomando dolly
            //som de heal
            //heal
            collision.gameObject.GetComponent<Character>().PowerUp();
            Destroy(this.gameObject);
        }
    }
}
