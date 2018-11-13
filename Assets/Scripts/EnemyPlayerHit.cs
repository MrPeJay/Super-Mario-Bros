using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();

            if (!player.amInvincible)
            {
                if (!player.isDead)
                {
                    if (!player.amHigh)
                    {
                        player.GrowDown();
                    }
                }
            }
        }
    }
}
