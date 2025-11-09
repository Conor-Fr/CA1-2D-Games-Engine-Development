using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script is used by the projectile object that the Enemies fire
public class Projectile : MonoBehaviour
{

    Rigidbody2D rbody;
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    //This section launches the projectile in direction the Enemy is facing moving along the y axis and moving with a given force
    public void Launch(Vector2 direction, float force)
    {
        rbody.AddForce(direction * force);
    }

    //This section has the projectile be destroyed if it collides with our player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
