using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour
{
    public QuestionType itemType;

    public GameObject GrowMushroom;
    public GameObject HealthMushroom;
    public GameObject DeathMushroom;
    public GameObject Flower;
    public GameObject Coin;
    public GameObject Star;

    public Sprite disabledImage;

    public AudioClip PowerUpSpawn;

    private int tenCoins = 10;

    private InteractableBlocks blocks;

    private void Start()
    {
        blocks = GetComponent<InteractableBlocks>();
    }

    public void ItemSelection(bool isGrown)
    {
        switch (itemType)
        {
            case QuestionType.Grow:
                if (isGrown)
                {
                    SpawnItem(Flower);
                    MusicController.PlayClipAt(PowerUpSpawn, transform.position);
                    SetToDisabled();
                }
                else
                {
                    SpawnItem(GrowMushroom);
                    MusicController.PlayClipAt(PowerUpSpawn, transform.position);
                    SetToDisabled();
                }
                break;
            case QuestionType.OneCoin:
                SpawnItem(Coin);
                SetToDisabled();
                break;
            case QuestionType.TenCoin:
                tenCoins--;
                SpawnItem(Coin);
                if (tenCoins <= 0)
                {
                    SetToDisabled();
                }
                break;
            case QuestionType.Health:
                SpawnItem(HealthMushroom);
                MusicController.PlayClipAt(PowerUpSpawn, transform.position);
                SetToDisabled();
                break;
            case QuestionType.Death:
                SpawnItem(DeathMushroom);
                MusicController.PlayClipAt(PowerUpSpawn, transform.position);
                SetToDisabled();
                break;
            case QuestionType.Star:
                SpawnItem(Star);
                MusicController.PlayClipAt(PowerUpSpawn, transform.position);
                SetToDisabled();
                break;
            default:
                break;
        }
    }

    private void SpawnItem(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn,transform.position,Quaternion.identity);
    }

    private void SetToDisabled()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = disabledImage;

        if (GetComponent<Animator>())
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = false;
        }

        blocks.type = BlockType.Emtpy;
    }
}
