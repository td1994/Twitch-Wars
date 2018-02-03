using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicEnemyScript : EnemyScript {
	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            float playX = player.transform.position.x, playY = player.transform.position.y, enemyX = this.transform.position.x, enemyY = this.transform.position.y;
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
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(sin * speed, cos * speed);
        }
	}
}
