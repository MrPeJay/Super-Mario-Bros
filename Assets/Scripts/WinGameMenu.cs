using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinGameMenu : MonoBehaviour {

    private GameManager manager;
    public Text score;

    private void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void ShowScore()
    {
        score.text = manager.GetScore().ToString();
    }
}
