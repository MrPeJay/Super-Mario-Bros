using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

    public float timeToDestroy;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, timeToDestroy);
	}
	
}
