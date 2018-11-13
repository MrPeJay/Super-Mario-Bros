using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnInteracted : MonoBehaviour {

    public float secondsToDisable = 0.1f;

    private void OnEnable()
    {
        StartCoroutine(disableInSeconds());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<EnemyStomp>())
        {
            EnemyStomp enemy = collision.GetComponentInChildren<EnemyStomp>();

            enemy.EnemyKill();
        }
    }

    IEnumerator disableInSeconds()
    {
        float time = secondsToDisable;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
