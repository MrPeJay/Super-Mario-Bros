using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private Collider2D groundCheckCollider;
    public Player player;

    private void Start()
    {
        groundCheckCollider = GetComponent<Collider2D>();   
    }

    private void FixedUpdate()
    {
        if (groundCheckCollider.IsTouchingLayers(player.layers))
        {
            player.amGrounded = true;
            player.animator.SetBool("IsGrounded", true);
        }
        else
        {
            player.amGrounded = false;
            player.animator.SetBool("IsGrounded", false);
            if (!player.amJumping)
            {
                player.player.AddForce(new Vector2(0, -player.downForce), ForceMode2D.Impulse);
            }
        }
    }
}
