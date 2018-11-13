using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

    public ItemType type;
    public int scoreToAdd = 1000;

    private GameManager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();

            switch (type)
            {
                case ItemType.Health:
                    manager.AddLives();
                    break;
                case ItemType.Grow:
                    player.GrowUp(false);
                    break;
                case ItemType.Death:
                    player.StartPlayerDeathCoroutine();
                    break;
                case ItemType.Flower:
                    player.GrowUp(true);
                    break;
                case ItemType.Star:
                    player.PlayerHigh();
                    break;
                default:
                    break;
            }
            manager.AddScore(scoreToAdd, transform.position,false);
            Destroy(transform.parent.gameObject);
        }
    }
}

public enum ItemType
{
    Health,
    Grow,
    Death,
    Flower,
    Star
}
