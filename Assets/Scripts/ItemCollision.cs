using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour {

    private ItemMovement movement;
    public float timeToRespawn = 3f;
    public GameObject Turtle;
    public bool isShell = false;

    private bool hasLeft = false;

    private void Start()
    {
        movement = GetComponentInParent<ItemMovement>();
        if (isShell)
        {
            movement.canMove = false;
            StartCoroutine(waitforRespawn());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (movement.canMove)
        {
            if (collision.GetComponentInParent<Player>())
            {
                hasLeft = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isShell)
        {
            movement.accelerationSpeed = -movement.accelerationSpeed;
        }
        else
        {
            if (movement.canMove)
            {
                if (collision.tag == "Enemy")
                {
                    EnemyStomp enemy = collision.GetComponentInChildren<EnemyStomp>();

                    enemy.EnemyKill();
                }
                else if (collision.GetComponentInParent<Player>())
                {
                    if (hasLeft)
                    {
                        Player player = collision.GetComponentInParent<Player>();
                        if (!player.isDead)
                        {
                            player.GrowDown();
                        }
                    }
                }
                else
                {
                    movement.accelerationSpeed = -movement.accelerationSpeed;
                }
            }
            else
            {
                if (collision.GetComponentInParent<Player>())
                {
                    hasLeft = false;
                    Vector3 toTarget = (collision.gameObject.transform.position - transform.position).normalized;
                    movement.canMove = true;
                    if (Vector3.Dot(toTarget, gameObject.transform.right) > 0)
                    {
                        if (movement.accelerationSpeed > 0)
                        {
                            movement.accelerationSpeed = -movement.accelerationSpeed;
                        }
                    }
                    else
                    {
                        if (movement.accelerationSpeed < 0)
                        {
                            movement.accelerationSpeed = -movement.accelerationSpeed;
                        }
                    }
                }
            }
        }
    }

    IEnumerator waitforRespawn()
    {
        float time = timeToRespawn;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        if (!movement.canMove)
        {
            Instantiate(Turtle, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}
