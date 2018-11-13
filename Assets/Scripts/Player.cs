using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player sizes")]
    public GameObject Small;
    public GameObject Grown;
    public GameObject SuperGrown;

    public int blinkIterations = 3;
    public float iterationTime = 0.5f;

    [Header("Movement Speed")]
    private float horizontalMove;
    public float acceleration;
    public float walkMaxSpeed;
    public float runMaxSpeed;

    public float stopForce;

    [Range(250,600)]
    public float multiplier;

    [Header("Jump Forces")]
    public float jumpForceSmall;
    public float jumpForceGrown;
    private float jumpForce;
    [Range(1,5)]
    public float downForce;

    public bool isGrown = false;
    public bool isDead = false;

    [HideInInspector]
    public bool amRunning = false;
    [HideInInspector]
    public bool amJumping = false;
    [HideInInspector]
    public bool amFacingRight = true;
    public bool amInvincible = false;
    public bool amHigh = false;

    [Header("Invincible Flashing")]
    public float invincibleTime = 1f;
    public float flashSpeed = 4;
    private float colorTime = 1f;
    public float maxFlash = 1f;
    public float minFlash = 0f;
    private bool hasChecked = false;

    [Header("Collision")]
    public LayerMask layers;
    [HideInInspector]
    public bool amGrounded = false;

    private float startJumpForce;
    [HideInInspector]
    public Rigidbody2D player;
    [HideInInspector]
    public Animator animator;

    private Collider2D[] colliders;

    private GameManager manager;

    [Header("Sounds")]
    public AudioClip jumpSmall;
    public AudioClip jumpGrown;
    public AudioClip death;
    public AudioClip PowerUp;
    public AudioClip Shrink;
    public AudioClip Shoot;

    public GameObject FireBall;
    public float timeBeforeNextShot = 0.25f;
    public float timeToBeHigh = 10f;

    private new SpriteRenderer renderer;
    private Color startColor;

    private bool canShoot = true;

    public bool ReactToPhysics = true;

    private void Awake()
    {
        startJumpForce = jumpForceSmall;
    }

    // Use this for initialization
    void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        player = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        SetStart();
	}
    private void ToogleColliders(bool enable)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = enable;
        }
    }

    private void SetStart()
    {
        animator = Small.GetComponent<Animator>();
        renderer = Small.GetComponent<SpriteRenderer>();
        startColor = renderer.color;
        isDead = false;
        ToogleColliders(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (ReactToPhysics)
        {
            if (!isDead)
            {
                horizontalMove = Input.GetAxisRaw("Horizontal") * acceleration;

                animator.SetFloat("Speed", Mathf.Abs(player.velocity.x));
                animator.speed = Mathf.Abs(player.velocity.x) / walkMaxSpeed;
            }

            if (!isGrown)
            {
                animator.SetBool("IsDead", isDead);
            }

            if (horizontalMove < 0)
            {
                if (amFacingRight)
                {
                    ChangeFaceSide();
                    amFacingRight = false;
                }
            }
            else if (horizontalMove > 0)
            {
                if (!amFacingRight)
                {
                    ChangeFaceSide();
                    amFacingRight = true;
                }
            }

            if (Input.GetButtonDown("Sprint"))
            {
                amRunning = true;
            }
            else if (Input.GetButtonUp("Sprint"))
            {
                amRunning = false;
            }

            if (Input.GetButtonDown("Shoot"))
            {
                if (SuperGrown.activeSelf)
                {
                    if (canShoot)
                    {
                        ShootFireBall();
                    }
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (amGrounded)
                {
                    amJumping = true;

                    if (isGrown)
                    {
                        MusicController.PlayClipAt(jumpGrown, transform.position);
                    }
                    else
                    {
                        MusicController.PlayClipAt(jumpSmall, transform.position);
                    }
                }
            }
            else if (Input.GetButtonUp("Jump"))
            {
                amJumping = false;
                ResetJump();
            }

            if (amHigh)
            {
                renderer.color = Random.ColorHSV(0, 1, .8f, 1, 0.9f, 1);
            }

            if (amInvincible)
            {
                Color color = startColor;
                color.a = colorTime;
                renderer.color = color;

                if (!hasChecked)
                {
                    colorTime += Time.unscaledDeltaTime * flashSpeed;
                    if (colorTime >= maxFlash)
                    {
                        colorTime = maxFlash;
                        hasChecked = true;
                    }
                }
                else
                {
                    colorTime -= Time.unscaledDeltaTime * flashSpeed;
                    if (colorTime <= minFlash)
                    {
                        colorTime = minFlash;
                        hasChecked = false;
                    }
                }
            }
        }
	}

    private void ShootFireBall()
    {
        GameObject @object = Instantiate(FireBall,transform.position,Quaternion.identity);
        Fireball fireball = @object.GetComponent<Fireball>();

        if (!amFacingRight)
        {
            Vector2 force = fireball.startForce;
            fireball.startForce = new Vector2(-force.x,force.y);   
        }

        MusicController.PlayClipAt(Shoot,transform.position);

        StartCoroutine(JustShot());
    }

    IEnumerator JustShot()
    {
        canShoot = false;
        float time = timeBeforeNextShot;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        canShoot = true;
    }

    private void ChangeFaceSide()
    {
        transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    public void ResetJump()
    {
        amJumping = false;
        jumpForce = startJumpForce;
    }


    private void FixedUpdate()
    {
        if (ReactToPhysics)
        {
            if (!amRunning)
            {
                if (amGrounded)
                {
                    if (player.velocity.x < walkMaxSpeed && player.velocity.x > -walkMaxSpeed)
                    {
                        player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                    }
                }
                else
                {
                    if (amFacingRight)
                    {
                        if (player.velocity.x < walkMaxSpeed)
                        {
                            player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                        }
                    }
                    else
                    {
                        if (player.velocity.x > -walkMaxSpeed)
                        {
                            player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                        }
                    }
                }
            }
            else
            {
                if (amGrounded)
                {
                    if (player.velocity.x < runMaxSpeed && player.velocity.x > -runMaxSpeed)
                    {
                        player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                    }
                }
                else
                {
                    if (amFacingRight)
                    {
                        if (player.velocity.x < runMaxSpeed)
                        {
                            player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                        }
                    }
                    else
                    {
                        if (player.velocity.x > -runMaxSpeed)
                        {
                            player.AddForce(new Vector2(horizontalMove * Time.deltaTime, 0));
                        }
                    }
                }
            }

            if (isDead)
            {
                if (player.velocity.x > 0)
                {
                    player.AddForce(new Vector2(-stopForce * Time.deltaTime, 0));
                }
                else
                {
                    player.AddForce(new Vector2(stopForce * Time.deltaTime, 0));
                }
            }

            if (amJumping)
            {
                player.AddForce(new Vector2(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
                if (jumpForce > 0)
                {
                    jumpForce -= Time.deltaTime * multiplier;

                    if (jumpForce <= 0)
                    {
                        ResetJump();
                        jumpForce = 0;
                    }
                }
            }
        }
    }

    public void GrowUp(bool canSuperGrow)
    {
        if (!isGrown)
        {
            isGrown = true;
            startJumpForce = jumpForceGrown;
            StartCoroutine(Blink(Grown, Small,true));
        }
        else
        {
            if (canSuperGrow)
            {
                StartCoroutine(Blink(SuperGrown, Grown, true));
            }
        }
        ResetColor();
        MusicController.PlayClipAt(PowerUp,transform.position);
    }

    public void GrowDown()
    {
        if (isGrown)
        {
            isGrown = false;
            startJumpForce = jumpForceSmall;
            if (Grown.activeSelf)
            {
                StartCoroutine(Blink(Small, Grown, true));
            }
            else
            {
                StartCoroutine(Blink(Small, SuperGrown, true));
            }
            StartCoroutine(Invincible());
            MusicController.PlayClipAt(Shrink,transform.position);
        }
        else
        {
            StartCoroutine(PlayerDead());
        }
    }

    private void ResetColor()
    {
        renderer.color = startColor;
    }

    IEnumerator Invincible ()
    {
        amInvincible = true;
        float time = invincibleTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        amInvincible = false;
        ResetColor();
    }

    IEnumerator High()
    {
        amHigh = true;
        manager.PlayTrack(manager.StarTheme);
        float time = timeToBeHigh;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        amHigh = false;
        manager.PlayTrack(manager.MainTheme);
        ResetColor();
    }

    public void StartPlayerDeathCoroutine()
    {
        StartCoroutine(FastDeath());
    }

    public void PlayerHigh()
    {
        StartCoroutine(High());
    }

    IEnumerator PlayerDead()
    {
        float time = manager.timeToDie;
        MusicController.MusicStop();
        MusicController.PlayClipAt(death,transform.position);
        ToogleColliders(false);
        isDead = true;
        player.AddForce(new Vector2(0,20),ForceMode2D.Impulse);

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        manager.PlayerDeath();
    }

    IEnumerator FastDeath()
    {
        float time = manager.timeToDie;
        MusicController.MusicStop();
        MusicController.PlayClipAt(death, transform.position);
        ToogleColliders(false);
        isDead = true;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        manager.PlayerDeath();
    }

    IEnumerator Blink(GameObject growTo, GameObject growFrom, bool stopTime)
    {
        if (stopTime)
        {
            Time.timeScale = 0;
        }
        int iteration = 0;
        bool blinked = false;

        while (iteration < blinkIterations)
        {
            float time = iterationTime;

            while (time > 0)
            {
                time -= Time.unscaledDeltaTime;
                yield return null;
            }

            if (blinked)
            {
                growTo.SetActive(false);
                growFrom.SetActive(true);
                animator = growFrom.GetComponent<Animator>();
                renderer = growFrom.GetComponent<SpriteRenderer>();
                ResetColor();
                blinked = false;
            }
            else
            {
                growTo.SetActive(true);
                growFrom.SetActive(false);
                animator = growTo.GetComponent<Animator>();
                renderer = growTo.GetComponent<SpriteRenderer>();
                ResetColor();
                blinked = true;
            }
            iteration++;

            yield return null;
        }

        Time.timeScale = 1;
    }
}
