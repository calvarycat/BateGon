using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

    // Use this for initialization
    public float timeToDestroy=5;
	void Start () {
        Destroy(this.gameObject, timeToDestroy);
	}
	
	
}
