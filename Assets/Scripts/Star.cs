using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    public Vector2 startForce;
    private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
	}

    public void PushMe()
    {
        rigid.AddForce(startForce, ForceMode2D.Impulse);
    }
}
