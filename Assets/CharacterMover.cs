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
    public Vector3 hitDirection;

    Animator anim;
    CharacterController cc;
    Vector2 moveInput = new Vector2();
    bool jumpInput;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");

        anim.SetFloat("Forwards", moveInput.y);
        anim.SetBool("Jump", !isGrounded);
    }
      
    private void FixedUpdate()
    {
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = cam.right;
        Vector3 delta = (moveInput.x * camRight + moveInput.y * camForward) * speed * Time.fixedDeltaTime;

        velocity += Physics.gravity * Time.fixedDeltaTime;

        if (jumpInput && isGrounded)
        {
            float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
            velocity.y = v;
        }
        
        if (isGrounded || moveInput.magnitude > 0)
        {
            velocity.x = delta.x;
            velocity.z = delta.z;
        }

        if (!isGrounded)        
            hitDirection = Vector3.zero;
        
        //TODO: may want to add a threshold in here as to when to start applying the force
        if(moveInput.x == 0 && moveInput.y == 0)
        {
            Vector3 horizontalHitDirection = hitDirection;
            horizontalHitDirection.y = 0;
            float displacement = horizontalHitDirection.magnitude;
            if (displacement > 0)
                velocity -= 0.2f * horizontalHitDirection / displacement;
        }

        delta += velocity * Time.fixedDeltaTime;
       
        isGrounded = cc.isGrounded;
        cc.Move(delta);
        transform.forward = camForward;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;
    }
}
