using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlocks : MonoBehaviour {

    public BlockType type;
    public BlockMovement movement;

    public AudioClip bump;
    public AudioClip destroy;

    public GameObject killOnInteractedCollider;
    public GameObject destroyBrickParticles;

    public void Interacted(bool isGrown)
    {
        Vector3 startPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + movement.moveUpDistance, transform.position.z);

        switch (type)
        {
            case BlockType.Brick:
                if (isGrown)
                {
                    MusicController.PlayClipAt(destroy,transform.position);
                    killOnInteractedCollider.SetActive(true);
                    Instantiate(destroyBrickParticles,transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }
                else
                {
                    MusicController.PlayClipAt(bump,transform.position);
                    killOnInteractedCollider.SetActive(true);
                }
                StartCoroutine(MoveUp(newPos, startPos));
                break;
            case BlockType.Question:
                ItemBlock block = GetComponent<ItemBlock>();
                block.ItemSelection(isGrown);
                StartCoroutine(MoveUp(newPos, startPos));
                killOnInteractedCollider.SetActive(true);
                break;
            case BlockType.Emtpy:
                break;
            case BlockType.Hidden:
                ItemBlock hiddenBlock = GetComponent<ItemBlock>();
                hiddenBlock.ItemSelection(isGrown);
                StartCoroutine(MoveUp(newPos, startPos));
                break;
            default:
                break;
        }
    }

    IEnumerator MoveUp(Vector3 endPos, Vector3 startPos)
    {
        while (transform.position != endPos)
        {
            float step = movement.moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position,endPos, step);
            yield return null;
        }

        StartCoroutine(MoveDown(startPos));
    }

    IEnumerator MoveDown(Vector3 startPos)
    {
        while (transform.position != startPos)
        {
            float step = movement.moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, startPos, step);
            yield return null;
        }
    }
}

public enum BlockType
{
    Brick,
    Question,
    Emtpy,
    Hidden
}

public enum QuestionType
{
    Grow,
    OneCoin,
    TenCoin,
    Health,
    Death,
    Star
}
