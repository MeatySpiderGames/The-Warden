 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float sens = 100f;
    private Transform playerTransform;
    private Transform cam;
    // The xRotation is the rotation of the main camera on the X Axis, or up and down (Used later to limit looking up and down)
    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        cam = Camera.main.GetComponent<Transform>();
        // Locks Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets Mouse X and Y Position (Whenever your mouse moves up, the Mouse X axis increases, as well as for the Mouse Y axis, it doesn't stop increasing even if your mouse is at the top of the screen)
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        // Vector3.up is just fancy for (0,1,0), so we are rotating on the Y Axis or left and right by mouseX
        playerTransform.Rotate(Vector3.up, mouseX);
        playerTransform.rotation = Quaternion.Euler(0f, playerTransform.rotation.eulerAngles.y, 0f);
        // Subtracts the mouse vertical movement from the xRotation or the player (I have no idea why it's not add but I'm too lazy to figure it out)
        xRotation -= mouseY;
        // Clamps xRotation so you can't look look backwards by looking down (Clamp means that if the first argument is less than the second, it sets the first to the second, and if the first argument is greater than the third, it sets it to the third)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Sets the local rotation (Rotation based on the parent/player, remember this is the camera we are rotating, we don't want to make the whole player rotate up) to xRotation, 0f, 0f (Quaternion.Euler is just fancy for "rotation that accepts three degree values")
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
