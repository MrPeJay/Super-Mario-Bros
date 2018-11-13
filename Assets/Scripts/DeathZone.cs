using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<Player>())
        {
            Destroy(collision.gameObject);
        }
        else
        {
            Player player = collision.GetComponentInParent<Player>();
            if (!player.isDead)
            {
                player.StartPlayerDeathCoroutine();
            }
        }
    }
}
