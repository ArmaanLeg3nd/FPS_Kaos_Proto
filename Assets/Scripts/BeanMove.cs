using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanMove : MonoBehaviour
{
    public float mspeed = 6f;
    public float grav = -10f;
    public bool isgrounded;
    public float jumpHeight = 30f;
    public Transform GCheck;
    public float groundDis = 0.4f;
    public LayerMask groundMask;
    public CharacterController parentcc;
    private Vector3 vel = Vector3.zero;
    public CapsuleCollider col;

    // This is a reference to character controller component for player.

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        parentcc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        isgrounded = Physics.CheckSphere(GCheck.position, groundDis, groundMask);

        if (isgrounded && vel.y < 0)
        {
            vel.y = -2f;
        }

        float a = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Vertical");

        //What basically these GetAxis attributes are doing that, they're checking if there is a Horizontal or Vertical Input every frame per second, like if you are pressing 'a' key, it means Horizontal input is -1.

        Vector3 c = transform.right * a + transform.forward * b;

        //This is basically moving Horizontally (left and right) and Vertically (forward and backwards) with respect to where you are looking.

        parentcc.Move(c * mspeed * Time.deltaTime);

        //This is using the reference created above for character controller, and use speed component in it and using Time.deltaTime makes it framerate independent.

        if (Input.GetButtonDown("Jump") && isgrounded)
        {
            vel.y = Mathf.Sqrt(jumpHeight * -2f * grav);
        }

        vel.y += grav * Time.deltaTime;

        parentcc.Move(vel * Time.deltaTime);

    }
}
