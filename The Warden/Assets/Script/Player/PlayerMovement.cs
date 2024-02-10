using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public float spd = 5f;
    public float spdSprint = 10f;
    public float spdStrafe = 2.5f;
    public float spdBack = 1f;
    public int jumpMagnitude = 10;
    private Rigidbody playerRb;
    private Transform playerTransform;
    private bool isPlayingSFX;
    private bool isPlayingFast;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        source = GetComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Audio/SFX/Walking");
        source.clip = clip;
        source.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (!isPlayingSFX)
            {
                isPlayingSFX = true;
                source.Play();
            }
            if (Input.GetKey(KeyCode.W))
            {
                // playerTransform.forward means 1 unit ahead of the player
                Vector3 forward = playerTransform.forward;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (!isPlayingFast)
                    {
                        // Plays at 2x speed
                        source.pitch = 2f;
                        isPlayingFast = true;
                    }
                    playerTransform.position += forward * Time.deltaTime * spdSprint;
                } else
                {
                    if (isPlayingFast)
                    {
                        source.pitch = 1f;
                        isPlayingFast = false;
                    }
                    playerTransform.position += forward * Time.deltaTime * spd;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 left = playerTransform.right * -1;
                playerTransform.position += left * Time.deltaTime * spdStrafe;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 backward = playerTransform.forward * -1;
                playerTransform.position += backward * Time.deltaTime * spdBack;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 right = playerTransform.right;
                playerTransform.position += right * Time.deltaTime * spdStrafe;
            }
        } else
        {
            if (isPlayingSFX)
            {
                isPlayingSFX = false;
                source.Stop();
            }
            if (isPlayingFast)
            {
                isPlayingFast = false;
            }
        }
        // GetKey registers true every frame the key is down, and GetKeyDown only registers the frame the key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Direction and magnitude
            playerRb.AddForce(Vector3.up * jumpMagnitude, ForceMode.Impulse);
        }
    }
}
