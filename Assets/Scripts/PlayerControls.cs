using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public int speed = 10;
    public GameObject projectile;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        float distFromCharacter = this.GetComponent<SpriteRenderer>().bounds.size.x * 0.75f + 0.1f;
        /*Debug.Log("Left Analog Stick Coordinates: x=" + Input.GetAxis("Horizontal") + ", y=" + Input.GetAxis("Vertical"));
        Debug.Log("Right Analog Stick Coordinates: x=" + Input.GetAxis("FireX") + ", y=" + Input.GetAxis("FireY"));*/

        if(Math.Abs(Input.GetAxis("Mouse X")) > 0 || Math.Abs(Input.GetAxis("Mouse Y")) > 0)
        {
            double totalIntensity = Math.Pow(Math.Abs(Input.GetAxis("Mouse X")), 2) + Math.Pow(Math.Abs(Input.GetAxis("Mouse Y")), 2);
            totalIntensity = 1 / totalIntensity;
            Vector2 pos = new Vector2(this.GetComponent<SpriteRenderer>().transform.position.x, this.GetComponent<SpriteRenderer>().transform.position.y);
            GameObject newProjectile = Instantiate(projectile, pos, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Mouse X") * (float)Math.Sqrt(totalIntensity) * speed * 2,
                Input.GetAxis("Mouse Y") * (float)Math.Sqrt(totalIntensity) * speed * 2);
        } else if(Math.Abs(Input.GetAxis("FireY")) > 0 || Math.Abs(Input.GetAxis("FireX")) > 0) {
            double totalIntensity = Math.Pow(Math.Abs(Input.GetAxis("FireX")), 2) + Math.Pow(Math.Abs(Input.GetAxis("FireY")), 2);
            totalIntensity = 1 / totalIntensity;
            Vector2 pos = new Vector2(this.GetComponent<SpriteRenderer>().transform.position.x, this.GetComponent<SpriteRenderer>().transform.position.y);
            GameObject newProjectile = Instantiate(projectile, pos, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("FireX") * (float)Math.Sqrt(totalIntensity) * speed * 2,
                Input.GetAxis("FireY") * (float)Math.Sqrt(totalIntensity) * speed * 2);
        }
	}

    private void OnDestroy()
    {
        //show Game Over Screen
    }
}
