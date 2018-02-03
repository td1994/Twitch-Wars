using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SniperScript : EnemyScript {

    private bool shootingPhase = false;
    private float time = 0f;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float playX = player.transform.position.x, playY = player.transform.position.y, enemyX = this.transform.position.x, enemyY = this.transform.position.y;

            //find the angle in which the enemy needs to move
            float adj = enemyY - playY;
            float opp = enemyX - playX;
            if (enemyY > playY)
            {
                adj = -1 * Math.Abs(adj);
            }
            else
            {
                adj = Math.Abs(adj);
            }

            if (enemyX > playX)
            {
                opp = -1 * Math.Abs(opp);
            }
            else
            {
                opp = Math.Abs(opp);
            }

            float hyp = (float)Math.Sqrt(Math.Pow(adj, 2) + Math.Pow(opp, 2));
            float sin = opp / hyp;
            float cos = adj / hyp;

            if (!shootingPhase)
            {
                if (hyp < 10f)
                {
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(cos * speed, sin * speed);
                } else
                {
                    shootingPhase = true;
                    time = Time.time;
                }
            } else
            {
                if(Time.time - time >= 1f)
                {
                    Debug.Log("Firing Bullet");
                    double totalIntensity = Math.Pow(Math.Abs(Input.GetAxis("Mouse X")), 2) + Math.Pow(Math.Abs(Input.GetAxis("Mouse Y")), 2);
                    totalIntensity = 1 / totalIntensity;
                    Vector2 pos = new Vector2(this.GetComponent<SpriteRenderer>().transform.position.x, this.GetComponent<SpriteRenderer>().transform.position.y);
                    GameObject newProjectile = Instantiate(projectile, pos, Quaternion.identity);
                    newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(sin * projectileSpeed, cos * projectileSpeed);
                    shootingPhase = false;    
                }
            }
        }
    }
}
