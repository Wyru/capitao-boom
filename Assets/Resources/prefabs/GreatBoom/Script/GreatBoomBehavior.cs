using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatBoomBehavior : MonoBehaviour {


    AudioSource explosion;

    public void Destroy() {
        Destroy(this.gameObject);
    }

}
