using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector2 direction = new Vector2(0,1);
    public float speed = 2;

    public Vector2 velocity;
	
	public bool isEnemy = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		if (isEnemy == true) {
			Destroy(gameObject, 6);
		}
		else {
        Destroy(gameObject, 1);
		}
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos += velocity * Time.fixedDeltaTime;
        transform.position = pos;
    }
}
