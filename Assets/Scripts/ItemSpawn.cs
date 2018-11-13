using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour {

    private ItemMovement movement;
    public BlockMovement blockMovement;
    private Rigidbody2D rigid;
    private Star star;
    private Collider2D[] colliders;

    public bool isRigid = true;
    public bool destroyOnEnd = false;
    public bool canMove = true;
    public bool moveDown = false;

    public bool isStar = false;

    private void Start()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        disableColliders(colliders);

        if (canMove)
        {
            movement = GetComponent<ItemMovement>();
        }

        if (isRigid)
        {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0;
        }

        if (isStar)
        {
            star = GetComponent<Star>();
        }

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x,transform.position.y + blockMovement.moveUpDistance, transform.position.z);

        StartCoroutine(MoveUp(startPos, endPos));
    }

    private void disableColliders(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    private void enableColliders(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    IEnumerator MoveUp(Vector3 startPos, Vector3 endPos)
    {
        while (transform.position != endPos)
        {
            float step = blockMovement.moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, endPos, step);
            yield return null;
        }

        if (canMove)
        {
            movement.canMove = true;
        }

        enableColliders(colliders);
        if (isRigid)
        {
            rigid.gravityScale = 1;
        }

        if (isStar)
        {
            star.PushMe();
        }

        if (moveDown)
        {
            StartCoroutine(MoveDown(startPos));
        }
        else
        {
            if (destroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator MoveDown(Vector3 startPos)
    {
        while (transform.position != startPos)
        {
            float step = blockMovement.moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, startPos, step);
            yield return null;
        }

        if (destroyOnEnd)
        {
            Destroy(gameObject);
        }
    }
}
