using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public int health = 1;
    public float speed = 5f;
    public float projectileSpeed = 0f;
    public GameObject player;
    public GameObject projectile;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Character");
    }

    void takeDamage()
    {
        health--;
        if (health == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Enemy Projectile" && collision.gameObject.tag != "Border")
        {
            Destroy(collision.gameObject);
            takeDamage();
        }
    }
}
