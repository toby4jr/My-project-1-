using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float moveSpeed = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		//destroys object if it exists for too long
       Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= moveSpeed * Time.fixedDeltaTime;

        if (pos.x < -2.0f)
        {
            Destroy(gameObject);
        }
        transform.position = pos;
    }
}
