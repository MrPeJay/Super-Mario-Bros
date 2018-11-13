using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour {

    public Sprite stompedImage;
    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D enemyRigid;
    private ItemMovement movement;
    private GameManager manager;
    private Collider2D[] colliders;

    public int scoreToAdd = 100;
    public float destroyTime = 2f;

    public AudioClip Stomp;
    public GameObject Shell;

    public EnemyType type;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        renderer = GetComponentInParent<SpriteRenderer>();
        enemyRigid = GetComponentInParent<Rigidbody2D>();
        movement = GetComponentInParent<ItemMovement>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        colliders = transform.parent.GetComponentsInChildren<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();
            Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();

            if (rigid.velocity.y < 0)
            {
                if (movement.canMove)
                {
                    rigid.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
                    MusicController.PlayClipAt(Stomp, transform.position);

                    switch (type)
                    {
                        case EnemyType.Goomba:
                            animator.enabled = false;
                            renderer.sprite = stompedImage;
                            movement.enabled = false;
                            ToogleColliders(false);
                            StartCoroutine(destroyInSeconds());
                            manager.AddScore(scoreToAdd, transform.position, true);
                            break;
                        case EnemyType.Turtle:
                            animator.enabled = false;
                            Instantiate(Shell, transform.position, Quaternion.identity);
                            manager.AddScore(scoreToAdd, transform.position, true);
                            Destroy(transform.parent.gameObject);
                            break;
                        case EnemyType.Shell:
                            movement.canMove = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (player.amHigh)
            {
                EnemyKill();
            }
        }
    }

    public void EnemyKill()
    {
        switch (type)
        {
            case EnemyType.Shell:
                break;
            case EnemyType.Goomba:
                renderer.sprite = stompedImage;
                animator.enabled = false;
                break;
            default:
                animator.enabled = false;
                break;
        }
        movement.enabled = false;
        enemyRigid.AddForce(new Vector2(0,5),ForceMode2D.Impulse);
        manager.AddScore(scoreToAdd, transform.position, true);
        ToogleColliders(false);
        MusicController.PlayClipAt(Stomp, transform.position);
        StartCoroutine(destroyInSeconds());
    }

    private void ToogleColliders(bool enable)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enable;
        }
    }

    IEnumerator destroyInSeconds()
    {
        float time = destroyTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(transform.parent.gameObject);
    }
}

public enum EnemyType
{
    Goomba,
    Turtle,
    Shell
}
