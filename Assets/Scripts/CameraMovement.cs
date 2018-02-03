using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraMovement : MonoBehaviour
{

    private float lastCameraPosX;
    private float lastCameraPosY;
    private GameObject player;
    private GameObject border;
    private float borderSpace;
    private int locationAt = 1;

    private const float INIT_DIST_X = 19.2f;
    private const float INIT_DIST_Y = 10.8f;

    void Start()
    {
        player = GameObject.Find("Character");
        border = GameObject.Find("Barrier");
    }

    void Update()
    {
        if ((player = GameObject.Find("Character")) != null)
        {
            PolygonCollider2D pc = border.GetComponent<PolygonCollider2D>();

            // get the coordinates of the player
            float posX = player.transform.position.x, posY = player.transform.position.y;

            if (Math.Abs((border.transform.position.x + pc.bounds.size.x / 2) - posX) <= INIT_DIST_X)
            {
                //collided with right barrier
                posX = (border.transform.position.x + pc.bounds.size.x / 2) - INIT_DIST_X;
            } else if (Math.Abs((border.transform.position.x - pc.bounds.size.x / 2) - posX) <= INIT_DIST_X)
            {
                //collided with left barrier
                posX = (border.transform.position.x - pc.bounds.size.x / 2) + INIT_DIST_X;
            }

            if(Math.Abs((border.transform.position.y + pc.bounds.size.y / 2) - posY) <= INIT_DIST_Y)
            {
                //collided with top barrier
                posY = (border.transform.position.y + pc.bounds.size.y / 2) - INIT_DIST_Y;
            } else if (Math.Abs((border.transform.position.y - pc.bounds.size.y / 2) - posY) <= INIT_DIST_Y)
            {
                //collided with bottom barrier
                posY = (border.transform.position.y - pc.bounds.size.y / 2) + INIT_DIST_Y;
            }

            //Debug.Log("X axis: " + posX + " Y axis: " + posY);
            this.transform.position = new Vector3(posX, posY, this.transform.position.z);
        }
    }
}
