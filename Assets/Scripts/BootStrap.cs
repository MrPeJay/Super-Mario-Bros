using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrap : MonoBehaviour {

    public float timeToStartLevel = 5;

	// Use this for initialization
	void Start () {
        StartCoroutine(startGameInSeconds());
	}

    IEnumerator startGameInSeconds()
    {
        float time = timeToStartLevel;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
	
}
