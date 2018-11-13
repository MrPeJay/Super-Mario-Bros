using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Vector2 startForce;
    private Rigidbody2D rigid;
    public GameObject destroyParticles;
    public AudioClip destroySound;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();

        rigid.AddForce(startForce,ForceMode2D.Impulse);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        MusicController.PlayClipAt(destroySound,transform.position);
    }
}
