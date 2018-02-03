using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((this.gameObject.tag == "Enemy Projectile" && collision.gameObject.tag == "Player") 
            || (this.gameObject.tag == "Enemy Projectile" && collision.gameObject.tag == "Enemy"))
        {
            Destroy(collision.gameObject);
        } 
        Destroy(this.gameObject);
    }
}
