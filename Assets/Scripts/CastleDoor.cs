using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDoor : MonoBehaviour {

    private Player player;
    private Rigidbody2D playerRigid;
    private GameManager manager;
    public AudioClip audio;
    public int scoreToAdd = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        player = collision.gameObject.GetComponentInParent<Player>();
        playerRigid = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        TooglePhysics(false);
        StartCoroutine(CountScore());
    }

    private void TooglePhysics(bool on)
    {
        player.ReactToPhysics = on;
        playerRigid.simulated = on;
    }

    IEnumerator CountScore()
    {
        manager.counting = true;

        while (manager.GetTime() > 1)
        {
            manager.MinusTime();
            manager.AddScore(scoreToAdd);
            MusicController.PlayClipAt(audio,transform.position);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        manager.WinGame();
    }
}
