using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float focusSpeedMultiplier = 0.5f;

    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb; 
    
    [SerializeField] private Animator animator; 
    [SerializeField] private GameObject visualHurtbox;

    [Header("Input Actions")]
    // Drag and drop your Input Actions from the Inspector here
    [SerializeField] private InputActionReference moveAction; 
    [SerializeField] private InputActionReference focusAction;
	[SerializeField] private InputActionReference attackAction;

    // State tracking
    private Vector2 moveInput;
    private bool isFocused;
    private bool isAttacking;
    Gun[] guns;

    void Start()
    {
		guns = transform.GetComponentsInChildren<Gun>();
    }

    private void Awake()
    {
        // Ensure we have the Rigidbody2D reference if not set in Inspector
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        
        // Optional: Ensure gravity doesn't pull your top-down character down
        rb.gravityScale = 0f; 
    }

    private void Update()
    {
        // 1. READ INPUT
        // The Input System automatically handles opposite-key cancellation if 
        // you use a "Up/Down/Left/Right Composite" for your Move action.
        moveInput = moveAction.action.ReadValue<Vector2>();
        isFocused = focusAction.action.IsPressed();
		isAttacking = attackAction.action.IsPressed();
		
		//Shooting weapons
		if (isAttacking)
		{
            FireWeapon();
		}

        // 2. HANDLE VISUALS (Animations & Hurtbox)
        bool isMoving = moveInput.sqrMagnitude > 0;
/*
        if (animator != null)
        {
            // Unity's closest equivalent to Godot's AnimatedSprite2D play()/stop()
            // Best practice is using Animator parameters like: animator.SetBool("IsMoving", isMoving);
            // But to directly mimic play/stop:
            animator.enabled = isMoving; 
        }

        if (visualHurtbox != null)
        {
            // Toggles visibility of the child GameObject
            visualHurtbox.SetActive(isFocused);
        }
        */
    }

    private void FixedUpdate()
    {
        // 3. APPLY PHYSICS MOVEMENT
        // We normalize to prevent faster diagonal movement (Vector2.ClampMagnitude also works)
        Vector2 direction = moveInput.normalized;
        
        // Apply focus speed reduction if shift is held
        float currentSpeed = isFocused ? speed * focusSpeedMultiplier : speed;

        // Apply velocity directly to the Rigidbody2D
        rb.linearVelocity = direction * currentSpeed;

        //clamp player within borders
        Vector2 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, 0.25f, 5.75f);
        pos.y = Mathf.Clamp(pos.y, -2.6f, 6.75f);
        rb.position = pos;
    }

    private void FireWeapon()
    {
        isAttacking = false;
		foreach(Gun gun in guns)
		{
			gun.Shoot();
            Debug.Log("Player score updated to:");
		}
        Debug.Log("Attacking!");
    }
    // Input actions need to be enabled/disabled when the script is turned on/off
    private void OnEnable()
    {
        moveAction.action.Enable();
        focusAction.action.Enable();
        attackAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        focusAction.action.Disable();
        attackAction.action.Disable();
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Bullet bullet = collision.GetComponent<Bullet>();
		if (bullet != null)
		{
			if (bullet.isEnemy)
			{
				Destroy(gameObject);
				Destroy(bullet.gameObject);
			}
		}
		
		Destructible destructible = collision.GetComponent<Destructible>();
		if (destructible != null)
		{
			Destroy(gameObject);
			Destroy(bullet.gameObject);
		}
	}
}