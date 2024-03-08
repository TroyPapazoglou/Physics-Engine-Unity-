using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplyer : MonoBehaviour
{
    Camera cam;
    public float force;
    private bool HitSomething;
    private Rigidbody rb;
    private Vector3 bodyForce;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = ~LayerMask.GetMask("Player") + ~LayerMask.GetMask("Platform");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Ragdoll ragDoll = hit.collider.GetComponentInParent<Ragdoll>();
                if (ragDoll != null)                
                    ragDoll.ragdollActive = true;                  
                    

                Rigidbody bodyPart = hit.collider.GetComponent<Rigidbody>();
                if (bodyPart != null)
                {
                    rb = bodyPart;
                    bodyForce = ray.direction * force;
                    hit.collider.GetComponent<Rigidbody>().velocity = (ray.direction * force);                    
                }
                    
            }            
        }


        
    }

    //private void FixedUpdate()
    //{
    //    if (HitSomething)
    //    {
    //        HitSomething = false;
    //        rb.AddForce(bodyForce);
    //        rb = null;
    //        bodyForce = Vector3.zero;
    //    }
    //}
}
