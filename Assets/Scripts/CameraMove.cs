using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public Transform beanBody;
    float xR = 0f;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //The GetAxis attribute is used to be able to rotate Mouse in X and Y direction.
        xR -= mouseY;
        xR = Mathf.Clamp(xR, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xR , 0f, 0f);
        beanBody.Rotate(Vector3.up * mouseX);
        //I don't really have much idea about rest of the stuff, but basically Mathf.clamp is making it so that we cannot look behind the character by going really up and down.
        //Quaternion Euler attribute makes it so that the rotation for the player is correct.
        //And what beanBody.Rotate does is makes the mouse rotation in front of player.
    }
}
