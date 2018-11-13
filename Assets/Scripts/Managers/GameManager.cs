using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int startScore = 0;
    public int startCoins = 0;
    public int startTime = 400;
    public int startLives = 3;

    public float timeToDie = 3f;

    public int score;
    private int coins;
    private float time;
    private int lives;

    public Text scoreText;
    public Text coinsText;
    public Text timeText;
    public Text livesText;

    public float timeToRestart = 5f;

    public Canvas WinGameCanvas;
    public Canvas gameOverCanvas;
    public Canvas UICanvas;

    public GameObject Text;

    private bool doublePoints = false;
    private bool addDoublePoints = false;
    public float timeToDouble = 2f;

    public AudioClip GameOverSound;
    public AudioClip OneUp;
    public AudioClip MainTheme;
    public AudioClip HurryTheme;
    public AudioClip StarTheme;
    public AudioClip WinTheme;

    private Vector3 POS;
    private Player player;

    public bool stopTimer = true;
    private bool stoppedTime = false;
    private bool hasPlayed = false;
    public GameObject PausedPrefab;

    public bool counting = false;

    private void Start()
    {
        RestartProgress();
    }

    public void RestartProgress()
    {
        score = startScore;
        coins = startCoins;
        time = startTime;
        lives = startLives;

        livesText.text = lives.ToString();
        scoreText.text = score.ToString();
        coinsText.text = coins.ToString();
        timeText.text = time.ToString();
    }

    public void RestartLive()
    {
        score = startCoins;
        time = startTime;

        scoreText.text = score.ToString();
        timeText.text = time.ToString();
    }

    private void Update()
    {
        if (!stopTimer)
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (!stoppedTime)
                {
                    Time.timeScale = 0;
                    Instantiate(PausedPrefab, transform.position,Quaternion.identity);
                    stoppedTime = true;
                }
                else
                {
                    Time.timeScale = 1;
                    GameObject @object = GameObject.FindGameObjectWithTag("Pause");
                    Destroy(@object);
                    stoppedTime = false;
                }
            }

            time -= Time.deltaTime * 2;

            timeText.text = Mathf.FloorToInt(time).ToString();
        }

        if (!counting)
        {
            if (time <= 0)
            {
                stopTimer = true;
                time = 0;
                timeText.text = Mathf.FloorToInt(time).ToString();

                if (!player.isDead)
                {
                    player.StartPlayerDeathCoroutine();
                }
            }
            else
            {
                if (time <= 91 && time >= 89)
                {
                    if (!hasPlayed)
                    {
                        HurryThemePlay();
                        hasPlayed = true;
                    }
                }
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            UICanvas.enabled = true;
            stopTimer = false;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            MusicController.PlayTrack(MainTheme);
        }
        else
        {
            UICanvas.enabled = false;
            stopTimer = true;
        }
    }

    private void HurryThemePlay()
    {
        MusicController.PlayTrack(HurryTheme);
    }

    public void PlayerDeath()
    {
        lives--;

        livesText.text = lives.ToString();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RestartLive();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void SpawnCoinText(Vector3 position, int score)
    {
        GameObject @object = Instantiate(Text, position, Quaternion.identity);
        TextMesh text = @object.GetComponent<TextMesh>();

        text.text = score.ToString();
    }

    public void AddLives()
    {
        lives++;
        livesText.text = lives.ToString();
        MusicController.PlayClipAt(OneUp,transform.position);
    }

    public void AddScore(int Score, Vector3 position, bool enemy)
    {
        POS = position;

        score += Score;
        scoreText.text = score.ToString();

        SpawnCoinText(POS, Score);
    }

    public void AddScore(int Score)
    {
        score += Score;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void PlayTrack(AudioClip audio)
    {
        MusicController.PlayTrack(audio);
    }


    public void AddCoin()
    {
        coins++;

        if (coins >= 100)
        {
            AddLives();

            coins = 0;
        }
        coinsText.text = coins.ToString();
    }

    public float GetTime()
    {
        return time;
    }

    public void MinusTime()
    {
        time--;
        timeText.text = Mathf.FloorToInt(time).ToString();
    }

    public void GameOver()
    {
        gameOverCanvas.enabled = true;
        stopTimer = true;
        MusicController.PlayClipAt(GameOverSound, transform.position);
        StartCoroutine(countToRestart(gameOverCanvas));
    }

    public void WinGame()
    {
        WinGameMenu menu = WinGameCanvas.gameObject.GetComponent<WinGameMenu>();
        menu.ShowScore();
        WinGameCanvas.enabled = true;
        StartCoroutine(countToRestart(WinGameCanvas));
    }

    IEnumerator countToRestart(Canvas canvas)
    {
        float time = timeToRestart;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        Destroy(gameOverCanvas.gameObject);
        Destroy(UICanvas.gameObject);
        SceneManager.LoadScene(0);
        canvas.enabled = false;
    }
}
