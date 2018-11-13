using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private Player player;
    public Rigidbody2D playerRigid;
    private bool hasInteractedHidden = false;
    public float timeToReset = 0.2f;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractableBlocks>())
        {
            InteractableBlocks block = collision.GetComponent<InteractableBlocks>();

            switch (block.type)
            {
                case BlockType.Hidden:
                    if (playerRigid.velocity.y > 0)
                    {
                        if (!hasInteractedHidden)
                        {
                            hasInteractedHidden = true;
                            HiddenBlock hidden = collision.GetComponent<HiddenBlock>();
                            hidden.Interacted();
                            block.Interacted(player.isGrown);
                            StartCoroutine(hiddenReset());
                        }
                    }
                    break;
                default:
                    block.Interacted(player.isGrown);
                    break;
            }
        }
        player.ResetJump();
    }

    IEnumerator hiddenReset()
    {
        float time = timeToReset;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        hasInteractedHidden = false;
    }
}
