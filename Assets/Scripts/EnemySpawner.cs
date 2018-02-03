using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private float startTime;
    public GameObject border;
    public GameObject enemy;
    public GameObject sniper;
    public GameObject charger;
    public GameObject nameText;

    private float minX, maxX, minY, maxY;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        minX = border.transform.position.x - border.GetComponent<PolygonCollider2D>().bounds.size.x / 2 + 0.5f;
        maxX = border.transform.position.x + border.GetComponent<PolygonCollider2D>().bounds.size.x / 2 - 0.5f;
        minY = border.transform.position.x - border.GetComponent<PolygonCollider2D>().bounds.size.y / 2 + 0.5f;
        maxY = border.transform.position.x + border.GetComponent<PolygonCollider2D>().bounds.size.y / 2 - 0.5f;
    }

    public void spawnEnemy()
    {
        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        Vector3 textPos = new Vector3(pos.x, pos.y, -1f);
        GameObject gen = Instantiate(enemy, pos, Quaternion.identity) as GameObject;
        GameObject text = Instantiate(nameText, textPos, Quaternion.identity) as GameObject;
        text.GetComponent<TextMesh>().text = "test";
        text.transform.parent = gen.transform;
    }

    public void spawnSniper()
    {
        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        Vector3 textPos = new Vector3(pos.x, pos.y, -1f);
        GameObject gen = Instantiate(sniper, pos, Quaternion.identity) as GameObject;
        GameObject text = Instantiate(nameText, textPos, Quaternion.identity) as GameObject;
        text.GetComponent<TextMesh>().text = "test";
        text.transform.parent = gen.transform;
    }

    public void spawnCharger()
    {
        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        Vector3 textPos = new Vector3(pos.x, pos.y, -1f);
        GameObject gen = Instantiate(charger, pos, Quaternion.identity) as GameObject;
        GameObject text = Instantiate(nameText, textPos, Quaternion.identity) as GameObject;
        text.GetComponent<TextMesh>().text = "test";
        text.transform.parent = gen.transform;
    }
}
