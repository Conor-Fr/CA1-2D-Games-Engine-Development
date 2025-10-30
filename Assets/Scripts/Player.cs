using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float JumpHeight;
    private bool jumping = false;
    private Animator animator;
    private int score = 0;

    [SerializeField] private UIManager ui;
    private Vector2 startPosition;
    private int lives = 3;
    private AudioSource _audio;
    private bool isPlaying = false;
    public AudioClip collectSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        float move = Input.GetAxis("Horizontal");

        if(move != 0 && !isPlaying && !jumping)
        {
            _audio.Play();
            isPlaying = true;
        }

        if (move == 0 && isPlaying || jumping)
        {
            _audio.Pause();
            isPlaying = false;
        }

        else
        {
            position.x = position.x + (speed * Time.deltaTime * move);
            transform.position = position;
        }
        updateAnimation(move);

        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
                rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * JumpHeight)),
                ForceMode2D.Impulse);
                jumping = true;
        }
    }

    private void updateAnimation(float move)
    {
        if(move > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if(move < 0)
        {
            animator.SetInteger("Direction", -1);
        }
        animator.SetFloat("Move", move);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            loseLife();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            Destroy(collision.gameObject);
            startPosition = transform.position;
        }
        if (collision.gameObject.name.Contains("Enemy Projectile"))
        {
            loseLife();
        }
    }

    private void loseLife()
    {
        lives--;
        if (lives <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
        ui.UpdateLives(lives);
        transform.position = startPosition;
    }

    public void CollectOre()
    {
        score++;
        ui.SetScore(score);
        _audio.PlayOneShot(collectSound);
    }
}
