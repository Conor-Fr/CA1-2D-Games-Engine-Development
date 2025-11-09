using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//This script is used by the Enemies to:
//-Move in a direction for a period of time then turn back around and come back the way they came
//-Fire a projectile if the enemy sees the Player
//-Take damage if the enemy is hit by the players melee
public class SlimeController : MonoBehaviour
{
    Animator _animator;
    int direction = 1;
    float timeInDirection;

    public float distanceTime;
    public float speed;
    public int health;
    public bool isDead = false;
    float dieTime = 1;
    bool isIdle = false;
    public float idleTime = 2;

    [SerializeField] float fireTimer = 0.5f;
    float fireCountdown = 0;
    [SerializeField] GameObject projectilePrefab;

    private AudioSource _audio;
    private bool isPlaying = false;
    public AudioClip hurtSound;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //This section of code will only perform if the Enemy is not dead
        if(!isDead)
        {
            //This section of the code performs the task of making the Enemy move from side to side
            if(isIdle && idleTime < 0)
            {
                direction = direction * -1;
                _animator.SetInteger("Direction", direction);
                _animator.SetFloat("Move", 1);
                timeInDirection = distanceTime;
                isIdle = false;

            }

            else if(!isIdle && timeInDirection < 0)
            {
                //direction = direction * -1;
                //_animator.SetInteger("DIrection", direction);
                idleTime = 2;
                isIdle = true;
                _animator.SetFloat("Move", 0);
            }
            if(!isIdle)
            {
                Vector2 pos = transform.position;
                pos.x = pos.x + (speed * Time.deltaTime * direction);
                transform.position = pos;
                timeInDirection -= Time.deltaTime;
            }
            else
            {
                idleTime -= Time.deltaTime;
            }

            //If the enemy sees the player they will fire with a cooldown inbetween to give the player a chance
            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                new Vector2(direction, 0), 5f, LayerMask.GetMask("Player"));
            if(hit.collider != null)
            {
                if(hit.collider.GetComponent<Player>() != null)
                {
                    fire();
                }
            }
            fireCountdown -= Time.deltaTime;
        }
        else
        {
            dieTime -= Time.deltaTime;
            //If the die timer ends meaning the death animation of the slime is finished the slime object will be destroyed
            if(dieTime < 0 )
            {
                Destroy(this.gameObject);
            }
        }
        //If the enemy is not in idle and is not playing then the audio is played of them walking
        if(!isIdle && !isPlaying)
        {
            _audio.Play();
            isPlaying = true;
        }
        //if the enemy is in idle and is playing then the audio of them walking is stopped
        if(isIdle && isPlaying)
        {
            _audio.Pause();
            isPlaying = false;
        }
    }

    //This code is read when the Enemy takes damage
    public void TakeDamage(int damage)
    {
        //The enemy loses a health from their starting 3
        health--;
        //The audio hurt sound is played to signal the player hit the enemy
        _audio.PlayOneShot(hurtSound);
        //if the health of the enemy hits 0 then the death boolean is set to true
        if (health <= 0)
        {
            isDead = true;
            //The death animation is then played
            _animator.SetBool("isDead", true);
           }
    }

    //The enemy with this code will fire a projectile when the fireCountdown hits zero in order to have the projectiles spread out
    public void fire()
    {
        if (fireCountdown < 0)
        {
            //reseting the timer for the next projectile
            fireCountdown = fireTimer;
            //Setting the projectile gameobject to the prefab projectile and set its position to the position of the Enemy gameobject
            GameObject projectile = Instantiate(projectilePrefab,
            GetComponent<Rigidbody2D>().position, Quaternion.identity);
            //Code to connect this script with the Projectile script
            Projectile script = projectile.GetComponent<Projectile>();
            //Projectile is launched in the direction the enemy is facing
            script.Launch(new Vector2(direction, 0), 300);
        }
    }
}
