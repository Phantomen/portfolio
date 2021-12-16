using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.
    [Tooltip("Player speed when holding shift")]
    [SerializeField] private float shiftSpeed = 3f;

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Rigidbody2D playerRigidbody;          // Reference to the player's rigidbody.

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        
    }


    void Update()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);
    }

    void Move(float h, float v)
    {

        // Set the movement vector based on the axis input.
        movement.Set(h, v, 0f);

        float s = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        { 
            s = shiftSpeed;
        }

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * s * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}