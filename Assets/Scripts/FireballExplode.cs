using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplode : MonoBehaviour {

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
}
