using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinOnSpawn : MonoBehaviour {

    public int scoreToAdd = 100;
    public AudioClip coin;

	// Use this for initialization
	void Start () {
        GameManager manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        manager.AddCoin();
        manager.AddScore(scoreToAdd,transform.position,false);

        MusicController.PlayClipAt(coin,transform.position);
    }
	
}
