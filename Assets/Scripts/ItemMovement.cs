using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour {

    public ItemMovementSO movement;
    [HideInInspector]
    public float accelerationSpeed;
    private Rigidbody2D rigid;
    public bool canMove = false;
    public bool startDirectionRight = true;

    public bool RotateObject = false;
    private bool facingRight = false;

    public float stopForce = 1000;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (startDirectionRight)
        {
            accelerationSpeed = movement.accelerationSpeed;
        }
        else
        {
            accelerationSpeed = -movement.accelerationSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (rigid.velocity.x < movement.maxSpeed && rigid.velocity.x > -movement.maxSpeed)
            {
                rigid.AddForce(new Vector2(accelerationSpeed * Time.deltaTime, 0), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (rigid.velocity.x > 0)
            {
                rigid.AddForce(new Vector2(-stopForce * Time.deltaTime, 0));
            }
            else
            {
                rigid.AddForce(new Vector2(stopForce * Time.deltaTime, 0));
            }
        }


        if (RotateObject)
        {
            if (accelerationSpeed > 0)
            {
                if (facingRight)
                {
                    ChangeFaceSide();
                    facingRight = false;
                }
            }
            else if (accelerationSpeed < 0)
            {
                if (!facingRight)
                {
                    ChangeFaceSide();
                    facingRight = true;
                }
            }
        }
    }

    private void ChangeFaceSide()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
