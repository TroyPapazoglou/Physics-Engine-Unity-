using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMover : MonoBehaviour
{
    //I can see youuuuu!
    [Tooltip("The speed the player can run up to")]
    public float speed = 10;
    [Tooltip("Player can jump to a height of X")]
    public float jumpHeight = 10;
    public float mass = 200;
    public Vector3 velocity;
    [Tooltip("")]
    public Vector3 hitDirection;
    [SerializeField] Vector2 moveInput = new Vector2();    
    public bool isGrounded;
        
    Transform cam;
    Animator anim;
    CharacterController cc;    
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
        velocity = (moveInput.x * camRight + moveInput.y * camForward) * speed * Time.fixedDeltaTime;

        

        if (isGrounded && this.velocity.y < 0)
            this.velocity.y = 0;

        if (jumpInput && isGrounded)
        {
            float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
            this.velocity.y = v;
        }

        this.velocity += Physics.gravity * Time.fixedDeltaTime;

        velocity += this.velocity * Time.fixedDeltaTime;

        if (!isGrounded)
            hitDirection = Vector3.zero;


        //When the player is in ragdoll mode the character controller collider is still on the slope
        //where as the actual model is off the slope but still moving 
        //consider fixing this or avoid this outcome of dying on a slope :)
        //if (moveInput.x == 0 && moveInput.y == 0)
        //{
        //    Vector3 horizontalHitDirection = hitDirection;
        //    horizontalHitDirection.y = 0;
        //    float displacement = horizontalHitDirection.magnitude;
        //    if (displacement > 0)
        //        velocity -= 0.2f * horizontalHitDirection / displacement;
        //}



        cc.Move(velocity);
        isGrounded = cc.isGrounded;
        transform.forward = camForward;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(velocity * mass, hit.point);
        }
    }
}
