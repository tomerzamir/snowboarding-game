using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 42f;
    [SerializeField] float jumpForce = 10f;
    private Rigidbody2D rb2d;
    private bool isGrounded = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
        }

        // Jumping control
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider's gameObject has the tag "snow"
        if (collision.gameObject.CompareTag("snow"))
        {
            // Set isGrounded to true when player touches snow
            Debug.Log("Tony is on the ground");
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset isGrounded to false when player leaves snow
        if (collision.gameObject.CompareTag("snow"))
        {
            Debug.Log("Tony is off the ground");
            isGrounded = false;
        }
    }

    void Jump()
    {
        // Add vertical velocity for jumping
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }
}
