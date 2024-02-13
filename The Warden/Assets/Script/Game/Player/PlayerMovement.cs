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
    private float PLAYERRADIUS = 0.5f;
    private float PLAYERHEIGHT = 2f;
    // forceStopDistance is how close in units the player can get to another object with a collider before it disallows movement in the direction of that object
    [SerializeField]
    private float forceStopDistance = 0.2f;
    // jumpSpare is how close in units the player has to be to the ground before he can jump again
    [SerializeField]
    private float jumpSpare = 0.2f;
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
                if (!ShootRays(playerTransform.forward, 45))
                {
                    // playerTransform.forward means 1 unit ahead of the player
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (!isPlayingFast)
                        {
                            // Plays at 2x speed
                            source.pitch = 2f;
                            isPlayingFast = true;
                        }
                        playerTransform.position += playerTransform.forward * Time.deltaTime * spdSprint;
                    }
                    else
                    {
                        if (isPlayingFast)
                        {
                            source.pitch = 1f;
                            isPlayingFast = false;
                        }
                        playerTransform.position += playerTransform.forward * Time.deltaTime * spd;
                    }
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (!ShootRays(playerTransform.right * -1, 45))
                {
                    playerTransform.position -= playerTransform.right * Time.deltaTime * spdStrafe;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (!ShootRays(playerTransform.forward * -1, 45))
                {
                    playerTransform.position -= playerTransform.forward * Time.deltaTime * spdBack;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (!ShootRays(playerTransform.right, 45))
                {
                    playerTransform.position += playerTransform.right * Time.deltaTime * spdStrafe;
                }
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
            if (Physics.Raycast(playerTransform.position, Vector3.down, out RaycastHit hitInfo, PLAYERHEIGHT / 2 + jumpSpare))
            {
                // Direction and magnitude
                playerRb.AddForce(Vector3.up * jumpMagnitude, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerTransform.localScale = new Vector3(1f, 0.7f, 1f);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        Debug.DrawLine(playerTransform.position, playerTransform.position + Vector3.down * (PLAYERHEIGHT / 2 + jumpSpare), Color.red, 0.1f);
    }
    // See ShootRay()
    bool ShootRays(Vector3 direction, int spread)
    {
        for (float i = -spread; i <= spread; i ++)
        {
            if (ShootRay(Quaternion.Euler(0f, i, 0f) * direction))
            {
                return true;
            }
        }
        return false;
    }
    // This shootray function is only for shooting a ray to the edge of the player horizontally
    bool ShootRay(Vector3 direction)
    {
        Debug.DrawLine(playerTransform.position, playerTransform.position + direction * (PLAYERRADIUS + forceStopDistance), Color.red, 0.1f);
        if (Physics.Raycast(playerTransform.position, direction, out RaycastHit hitInfo, PLAYERRADIUS + forceStopDistance))
        {
            return true;
        }
        return false;
    }
}
