using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SlimeController : MonoBehaviour
{
    Animator _animator;
    int direction = 1;
    float timeInDirection;

    public float distanceTime;
    public float speed;
    public int health;
    bool isDead = false;
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
        if(!isDead)
        {
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
            if(dieTime < 0 )
            {
                Destroy(this.gameObject);
            }
        }
        if(!isIdle && !isPlaying)
        {
            _audio.Play();
            isPlaying = true;
        }
        if(isIdle && isPlaying)
        {
            _audio.Pause();
            isPlaying = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health--;
        _audio.PlayOneShot(hurtSound);
        if (health <= 0)
        {
            isDead = true;
            _animator.SetBool("isDead", true);
           }
    }
    public void fire()
    {
        if (fireCountdown < 0)
        {
            fireCountdown = fireTimer;
            GameObject projectile = Instantiate(projectilePrefab,
            GetComponent<Rigidbody2D>().position, Quaternion.identity);
            Projectile script = projectile.GetComponent<Projectile>();
            script.Launch(new Vector2(direction, 0), 300);
        }
    }
}
