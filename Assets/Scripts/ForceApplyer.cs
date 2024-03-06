using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplyer : MonoBehaviour
{
    Camera cam;
    public float force;    

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
            LayerMask layerMask = ~LayerMask.GetMask("Player");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Ragdoll ragDoll = hit.collider.GetComponentInParent<Ragdoll>();
                if (ragDoll)
                    ragDoll.ragdollActive = true;

                Rigidbody body = hit.collider.GetComponent<Rigidbody>();
                if (body)
                    body.AddForce(-ray.direction * force);
            }            
        }


        
    }
}
