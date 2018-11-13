using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    public int scoreToAdd = 20;

    private Player player;
    private Rigidbody2D playerRigid;
    private Animator playerAnimator;
    public float moveSpeed;
    public Transform poleEnd;
    public Transform PointToMove;
    public AudioClip audio;
    private bool hasReachedEnd = false;
    private Vector3 startPos;
    private GameManager manager;
    private Collider2D[] colliders;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        colliders = GetComponents<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        startPos = new Vector2(transform.position.x,collision.GetContact(0).point.y);
        player = collision.gameObject.GetComponentInParent<Player>();
        player.transform.position = startPos;
        playerRigid = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        TooglePhysics(false);
        manager.stopTimer = true;
        MusicController.MusicStop();
        MusicController.PlayTrack(audio);
        StartCoroutine(MoveToPosition(true,poleEnd.position,false));
    }

    private void TooglePhysics(bool on)
    {
        player.ReactToPhysics = on;
        playerRigid.simulated = on;
    }

    private void ToogleColliders(bool on)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = on;
        }
    }


    IEnumerator MoveToPosition(bool addScore,Vector3 endPos,bool callEvent)
    {
        while (player.transform.position != endPos)
        {
            float step = moveSpeed * Time.deltaTime;

            player.transform.position = Vector2.MoveTowards(player.transform.position, endPos, step);
            if (addScore)
            {
                manager.AddScore(scoreToAdd);
            }

            yield return null;
        }
        if (callEvent)
        {
            Event();
        }
        if (!hasReachedEnd)
        {
            StartCoroutine(MoveToPosition(false,PointToMove.position,true));
        }
        hasReachedEnd = true;
    }

    private void Event()
    {
        MusicController.MusicStop();
        TooglePhysics(true);
        ToogleColliders(false);
        hasReachedEnd = true;
        MusicController.PlayTrack(manager.WinTheme);
    }
}
