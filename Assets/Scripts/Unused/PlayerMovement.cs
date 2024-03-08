using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float yVelocity;

    [SerializeField]
    private float mass = 200;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private bool isGrounded;
         
    private Vector2 playerInput;       
    private CharacterController cc;

    [SerializeField]
    private Vector3 moveDirection;
    private Vector3 hitDirection;

    Vector3 camForward;
    Vector3 camRight;
    
    Transform cam;
    Animator anim;


    private void Awake()
    {      
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        camForward = cam.forward;
        camRight = cam.right;
        
    }
          
   
    void Update()
    {
        velocity = cc.velocity;
        

        
        if (isGrounded && cc.velocity.y < 0)
        {
            yVelocity = -1.0f;
        }
        else
        {
            yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        moveDirection.y = yVelocity;

        
        cc.Move(Speed * Time.deltaTime * moveDirection);





        anim.SetFloat("Forwards", moveDirection.z);
        anim.SetBool("Jump", !isGrounded);







        if (!isGrounded)
            hitDirection = Vector3.zero;

        isGrounded = cc.isGrounded;
    }
     

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
        moveDirection = new Vector3(playerInput.x, 0f, playerInput.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded) return;

        float v = Mathf.Sqrt(2 * Physics.gravity.magnitude * jumpHeight);
        yVelocity = v;
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
