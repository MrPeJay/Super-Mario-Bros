using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject EnemyToSpawn;
    public float distanceToSpawn;
    private Player player;
    private bool spawned = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
        if (!spawned)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= distanceToSpawn)
            {
                Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
                spawned = true;
            }
        }
	}
}
