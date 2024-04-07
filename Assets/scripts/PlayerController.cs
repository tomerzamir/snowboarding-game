using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Import Unity UI namespace

public class PlayerController : MonoBehaviour
{
    float torqueAmount = 88f;
    [SerializeField] float jumpForce = 4.2f;
    private Rigidbody2D rb2d;
    private bool isGrounded = false;
    SurfaceEffector2D sfe2d;
    [SerializeField] float baseSpeed = 14f;
    [SerializeField] float boostSpeed = 36f;
    float boostDuration = 1f; // Duration of the boost in seconds
    private bool isBoosting = false; // Flag to track if the boost is currently active
    private bool isSnowball = false;
    [SerializeField] float slowSpeed = 6f;
    [SerializeField] ParticleSystem SnowParticles;
    [SerializeField] AudioClip windSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip spinSFX;
    // [SerializeField] float spinThreshold = 0.1f; // Threshold for a valid spin (adjust as needed)
    [SerializeField] int spinScore = 100; // Score awarded for each spin (adjust as needed)
    [SerializeField] int boostScore = 10; // Score awarded for each spin (adjust as needed)
    // [SerializeField] TextMeshProUGUI scoreText;


    private int totalScore = 0;

    // private float spinAngle = 0f; // Track the accumulated spin angle
    // keeps the last frames right vector
    private Vector2 _previousRight;

    // keeps the already rotated angle
    private float _angle;
    bool canMove = true;
    public void disableControls()
    {
        canMove = false;
    }
    void Jump()
    {
        // Add vertical velocity for jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            GetComponent<AudioSource>().PlayOneShot(jumpSFX);
        }
    }
    void rotatePlayer()
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
    }
    // public void UpdateScoreText()
    // {
    //     // Update the displayed score
    //     scoreText.text = "Score: " + totalScore;
    // }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sfe2d = FindObjectOfType<SurfaceEffector2D>();
        _previousRight = transform.right;
        // UpdateScoreText();
    }
    void PerformSpin()
    {
        // Ensure the spin meets the threshold to count as a scored spin
        if (!isGrounded)
        {
            GetComponent<AudioSource>().PlayOneShot(spinSFX);

            // Award a flat score for each completed spin
            int score = spinScore;
            totalScore += score;

            // // Update UI or do something with the score
            Debug.Log("Spin Score: " + score);
            Debug.Log("Total Score: " + totalScore);
            // UpdateScoreText();

            // // Subtract a full spin from accumulated angle
            // spinAngle -= 360f;
        }
    }
    public void PrintScore()
    {
        Debug.Log("Total Score: " + totalScore);
    }

    void Update()
    {
        if (canMove)
        {
            rotatePlayer();
            Jump();
            if (isBoosting || isSnowball)
            {
                boostDuration -= Time.deltaTime; // Decrease the duration over time

                if (boostDuration <= 0)
                {
                    EndBoost(); // If boost duration is over, end the boost
                }
            }
            // get this frame's right vector
            var currentRight = transform.right;

            // compare it to the previous frame's right vector and sum up the delta angle
            _angle += Vector2.SignedAngle(_previousRight, currentRight);

            // store the current right vector for the next frame to compare
            _previousRight = currentRight;

            // did the angle reach +/- 360 ?
            if (Mathf.Abs(_angle) >= 360f)
            {
                Debug.Log("Completed Full Spin!");
                PerformSpin();
                // if _angle > 360 subtract 360
                // if _angle < -360 add 360
                _angle -= 360f * Mathf.Sign(_angle);
            }
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
            GetComponent<AudioSource>().Stop();
            SnowParticles.Play();
        }

        if (collision.gameObject.CompareTag("snowBall"))
        {
            Debug.Log("you hit a snowball..slow down!");
            slow();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset isGrounded to false when player leaves snow
        if (collision.gameObject.CompareTag("snow"))
        {
            Debug.Log("Tony is off the ground");
            isGrounded = false;
            GetComponent<AudioSource>().PlayOneShot(windSFX);
            SnowParticles.Stop();
        }
    }
    void boost()
    {
        sfe2d.speed = boostSpeed;
        isBoosting = true; // Start the boost
        totalScore += boostScore;
        Debug.Log("extra points for boosting: " + boostScore);
        Debug.Log("total score: " + totalScore);
    }
    void slow()
    {
        sfe2d.speed = slowSpeed;
        isSnowball = true;
    }
    void EndBoost()
    {
        Debug.Log("back to normal speed");
        sfe2d.speed = baseSpeed; // Reduce speed to revert the boost effect
        isBoosting = false; // Reset the flag
        isSnowball = false;
        boostDuration = 1f; // Reset the duration for the next boost
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "boost" && !isBoosting) // Ensure the boost is not active before applying another boost
        {
            Debug.Log("here's some extra speed!");
            boost();
        }
    }

}
