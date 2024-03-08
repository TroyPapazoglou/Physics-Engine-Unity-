using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMover : MonoBehaviour
{    
    [Tooltip("The speed the player can run up to")]
    [SerializeField]
    private float speed = 10;

    [Tooltip("Player can jump to a height of X")]
    [SerializeField]
    private float jumpHeight = 10;

    [Tooltip("Defines the mass of the object")]
    [SerializeField]
    private float mass = 200;

    [Tooltip("Displays the current velocity of the object")]
    [SerializeField]
    private Vector3 velocity;

    [Tooltip("The position where the player and an object last collided")]
    [SerializeField]
    private Vector3 hitDirection;
            
    [Tooltip("Shows whether the player is currently grounded")]
    [SerializeField]
    private bool isGrounded;

    private Vector2 moveInput;
    bool jumpInput;
    Transform cam;
    Animator anim;
    CharacterController cc;        
    Ragdoll ragdoll;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
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
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camForward.Normalize();        
        Vector3 movementDirection = (moveInput.x * camRight + moveInput.y * camForward) * speed * Time.fixedDeltaTime;

        if (isGrounded || moveInput.x != 0 || moveInput.y != 0)
        {
            velocity.x = movementDirection.x;
            velocity.z = movementDirection.z;
        }

        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        if (jumpInput && isGrounded)
        {
            float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
            velocity.y = v;
        }

        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        velocity += Physics.gravity * Time.fixedDeltaTime;

        movementDirection += velocity * Time.fixedDeltaTime;

        if (!isGrounded)
            hitDirection = Vector3.zero;

        if (!ragdoll.ragdollActive)
        {
            if (moveInput.x == 0 && moveInput.y == 0)
            {
                Vector3 horizontalHitDirection = hitDirection;
                horizontalHitDirection.y = 0;
                float displacement = horizontalHitDirection.magnitude;
                if (displacement > 0)
                    velocity -= 0.2f * horizontalHitDirection / displacement;
            }
        }

        if (!ragdoll.ragdollActive)
        {
            cc.Move(movementDirection);
            isGrounded = cc.isGrounded;
            transform.forward = camForward;
        }       
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;
        if (hit.rigidbody)
        {
            if (!hit.gameObject.CompareTag("Platform"))
            {                
                hit.rigidbody.AddForceAtPosition(velocity * mass, hit.point);
            }            
           
        }
    }
}
