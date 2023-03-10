using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody Rb;
    public float jumpForce = 112f;
    public float groundCheckDistance = 0.3f;
    private bool isOnGround = false;
    public GameObject playerDead;
    AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        jumpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Physics.Raycast(
                transform.position + (Vector3.up * 0.1f),
                Vector3.down,
                groundCheckDistance
            )
        )
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }

        // User input using arrow keys to control player
        if (isOnGround)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AdjustPositionAndRotation(new Vector3(0, 0, 0));
                Rb.AddForce(new Vector3(0, jumpForce, jumpForce));
                jumpSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AdjustPositionAndRotation(new Vector3(0, 180, 0));
                Rb.AddForce(new Vector3(0, jumpForce, -jumpForce));
                jumpSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AdjustPositionAndRotation(new Vector3(0, -90, 0));
                Rb.AddForce(new Vector3(-jumpForce, jumpForce, 0));
                jumpSound.Play();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AdjustPositionAndRotation(new Vector3(0, 90, 0));
                Rb.AddForce(new Vector3(jumpForce, jumpForce, 0));
                jumpSound.Play();
            }
        }
    }

    // Adjust directions of hops of the player
    void AdjustPositionAndRotation(Vector3 newRotation)
    {
        Rb.velocity = Vector3.zero;
        transform.eulerAngles = newRotation;
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Mathf.Round(transform.position.z)
        );
    }

    // Trigger events when player hits something
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Step Trigger"))
        {
            LevelManager.levelManager.SetSteps();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Death Trigger"))
        {
            GameOver();
        }
    }

    // Collision events when something is hit by the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death Trigger"))
        {
            GameOver();
        }
    }

    // Game Over Screen
    void GameOver()
    {
        Instantiate(playerDead, transform.position, transform.rotation);
        gameObject.SetActive(false);
        LevelManager.levelManager.GameOver();
    }
}
