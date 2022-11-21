using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public int ammo { get { return currentAmmo; }}
    public int currentAmmo;

    public AudioClip cogThrowClip;
    public AudioClip HitPlayer;
    public AudioSource BackgroundMusic;
    public AudioClip WinSound;
    public AudioClip LoseSound;

    public ParticleSystem DamageEffect;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;
    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    public GameObject AuthorTextObject;
    bool gameOver;
    bool winGame;
    public static int level = 1;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        fixedText.text = "Robots Fixed: " + scoreFixed.ToString() + "/4";

        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        AuthorTextObject.SetActive(false);
        gameOver = false;
        winGame = false;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();

            if (currentAmmo > 0)
            {
                ChangeAmmo(-1);
                AmmoText();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    if (scoreFixed >= 4)
                    {
                        SceneManager.LoadScene("Level 2");
                        level = 2;
                    }

                    else
                    {
                    character.DisplayDialog();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (winGame == true)
            {
                SceneManager.LoadScene("Level 1");
                level = 1;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            audioSource.PlayOneShot(HitPlayer);

            DamageEffect = Instantiate(DamageEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        if (currentHealth == 1)
        {
            LoseTextObject.SetActive(true);
            AuthorTextObject.SetActive(true);

            transform.position = new Vector3(-5f,0f,-100f);
            speed = 0;
            Destroy(gameObject.GetComponent<SpriteRenderer>());

            gameOver = true;

            BackgroundMusic.Stop();

            audioSource.PlayOneShot(LoseSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void ChangeAmmo(int amount)
    {
        currentAmmo = Mathf.Abs(currentAmmo + amount);
        Debug.Log("Ammo: " + currentAmmo);
    }

    public void AmmoText()
    {
        ammoText.text = "Cogs: " + currentAmmo.ToString();
    }
    void Launch()
    {
        if (currentAmmo > 0)
        {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        audioSource.PlayOneShot(cogThrowClip);
        }
    }

      public void FixedRobots(int amount)
    {
        scoreFixed += amount;
        fixedText.text = "Robots Fixed: " + scoreFixed.ToString() + "/4";

        Debug.Log("Robots Fixed: " + scoreFixed);

        if (scoreFixed == 4 && level == 1)
        {
            WinTextObject.SetActive(true);
        }

        if (scoreFixed == 4 && level == 2)
        {
            WinTextObject.SetActive(true);
            AuthorTextObject.SetActive(true);

            winGame = true;

            transform.position = new Vector3(-5f, 0f, -100f);
            speed = 0;

            Destroy(gameObject.GetComponent<SpriteRenderer>());

            BackgroundMusic.Stop();

            audioSource.PlayOneShot(WinSound);
        }
        
    }
}

