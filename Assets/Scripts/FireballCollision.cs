using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCollision : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStomp enemy = collision.GetComponentInChildren<EnemyStomp>();
        GameObject parent = transform.parent.gameObject;

        enemy.EnemyKill();
        Destroy(parent);
    }
}
