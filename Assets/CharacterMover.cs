using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMover : MonoBehaviour
{
    public float speed = 10;
    public Vector3 velocity;
    [Tooltip("Player can jump to a height of X")]
    public float jumpHeight = 10;
    public bool isGrounded;
    Transform cam;

    CharacterController cc;
    Vector2 moveInput = new Vector2();
    bool jumpInput;
   
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = cam.right;
        Vector3 delta = (moveInput.x * Vector3.right + moveInput.y * Vector3.forward) * speed * Time.fixedDeltaTime;
        if(isGrounded || moveInput.x != 0 || moveInput.y != 0)
        {
            velocity.x = delta.x;
            velocity.z = delta.z;
        }
                

        if (jumpInput && isGrounded)
        {
            float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
            velocity.y = v;
            
        }

        velocity += Physics.gravity * Time.fixedDeltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        delta += velocity * Time.fixedDeltaTime;

        isGrounded = cc.isGrounded;
        cc.Move(delta);
    }
}
