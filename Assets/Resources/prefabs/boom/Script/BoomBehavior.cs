using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBehavior : MonoBehaviour {


    AudioSource explosion;

    public void Destroy() {
        Destroy(this.gameObject);
    }

}
