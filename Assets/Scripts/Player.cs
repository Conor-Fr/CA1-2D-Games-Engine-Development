using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script is used by the player entity to:
//-play audio when walking and stopping
//-jump when the space bar key is pressed
//-Stop the player from spamming melee
//-Melee when the F key is pressed
//-Set the different animations based on the Move and Direction variables
//-Come in contact with a checkpoint and have that become their new respawn point when hit
//-Player lose a life if collides with Enemy projectile
//-Lose a life and if lives <= 0 end the game
//-Increase score by collecting Diamonds and win the game if they collect all three diamonds
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
    private int lives = 5;
    private AudioSource _audio;
    private bool isPlaying = false;
    public AudioClip collectibleSound;

    //This section is used to create the melee variables
    public Transform meleePoint;
    public float meleeRange = 0.5f;
    public LayerMask enemyLayers;
    public float meleeRate = 2f;
    float nextMeleeTime = 0f;
    public AudioClip meleeSound;

    //This is the checkpoint section
    public AudioClip checkpointSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        _audio = GetComponent<AudioSource>();
        //meleeSound.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        float move = Input.GetAxis("Horizontal");

        if (move != 0 && !isPlaying && !jumping)
        {
            //Walking audio is played
            _audio.Play();
            isPlaying = true;
        }

        if (move == 0 && isPlaying)
        {
            //Walking audio stops
            _audio.Pause();
            isPlaying = false;
        }

        else
        {
            position.x = position.x + (speed * Time.deltaTime * move);
            transform.position = position;
        }
        updateAnimation(move);
        
        //This section will have the player jump when the press the space bar
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * JumpHeight)),
            ForceMode2D.Impulse);
            jumping = true;
        }

        //This if statement is to make sure that the player cannot just spam the melee button and instead must allow for a small window inbetween melee's
        if(Time.time >= nextMeleeTime)
        {
            //This code makes the player melee when the F key is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                melee();
                nextMeleeTime = Time.time + 1f / meleeRate;
                _audio.PlayOneShot(meleeSound);
            }
        }
    }

    //This section is used to control the different animations and setting the Direction and Move variables
    private void updateAnimation(float move)
    {
        if (move > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (move < 0)
        {
            animator.SetInteger("Direction", -1);
        }
        animator.SetFloat("Move", move);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
    }

    //This section controlls what happens when the player collides with an object
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player comes in contact with one of the Cauldron's which are the checkpoints this will become the new point where they spawn after taking damage
        if (collision.gameObject.tag == "Checkpoint")
        {
            Destroy(collision.gameObject);
            startPosition = transform.position;
            _audio.PlayOneShot(checkpointSound);
        }

        //this section make sthe player lose a life if they are hit by an enemy projectile
        if (collision.gameObject.name.Contains("EnemyProjectile"))
        {
            loseLife();
        }
    }


    //This section of code outlines what happens when the player loses a life
    private void loseLife()
    {
        lives--;
        //This section will load the main menu if the players lives reach 0
        if (lives <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
        ui.UpdateLives(lives);
        //The player is teleported back to the last place they got a checkpoint or their spawn location
        transform.position = startPosition;
    }


    //This section outlines what happens when the player collects one of the diamonds
    public void CollectOre()
    {
        //Adding 1 to the total score
        score++;
        //Updating the coutner in the top left corner
        ui.SetScore(score);
        //Play the collectibleSound audio clip
        _audio.PlayOneShot(collectibleSound);
        //If the score reaches 3 the player will be brought to the Game Won Scene
        if (score >= 3)
        {
            SceneManager.LoadScene("Game Won");
        }
    }

    //Thi section is used to allow the player to melee the slimes(Enemies)
    public void melee()
    {
        //Play the melee animation
        animator.SetTrigger("Melee");
        
        //Setting up the hitbox for the enemies and connecting it to the meleePoint position
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);
        //If the player melees and its within the enemies hitbox then the enemy will take damage
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<SlimeController>().TakeDamage(1);
        }
    }
}
