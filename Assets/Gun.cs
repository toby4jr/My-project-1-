using UnityEngine;

public class Gun : MonoBehaviour
{

    public Bullet bullet;
	Vector2 direction;
	
	public bool autoShoot = false;
	public float shootIntervalSeconds = 0.5f;
	public float shootDelaySeconds = 0.0f;
	float shootTimer = 0f;
	float delayTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = (transform.localRotation * Vector2.right).normalized;
		
		if (autoShoot)
		{
			if (delayTimer >= shootDelaySeconds)
			{
				if (shootTimer >= shootIntervalSeconds)
				{
					Shoot();
					shootTimer = 0;
					Debug.Log("FACK YOUUUUU");
				}
				else
				{
					shootTimer += Time.deltaTime;
				}
			}
			else
			{
				delayTimer += Time.deltaTime;
			}
		}
    }

    public void Shoot()
    {
        GameObject go =Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
    }
}

