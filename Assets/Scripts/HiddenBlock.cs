using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlock : MonoBehaviour {

    private Collider2D[] colliders;

    private void Start()
    {
        colliders = GetComponents<Collider2D>();
    }

    public void Interacted()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].isTrigger)
            {
                colliders[i].enabled = false;
            }
            else
            {
                colliders[i].enabled = true;
            }
        }
    }
}
