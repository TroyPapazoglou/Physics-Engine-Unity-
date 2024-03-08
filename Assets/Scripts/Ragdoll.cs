using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
    private Animator animator = null;   
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    public bool ragdollActive = false;
   
    public bool RagDoll
    {
        get { return !animator.enabled; }
        set
        {
            animator.enabled = !value;
            foreach (Rigidbody r in rigidbodies)
            {
                r.isKinematic = !value;
                r.WakeUp();
            }
            
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        CollectRigidbodiesInChildren(transform);

        foreach (Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
        }
    }

    void CollectRigidbodiesInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rigidbodies.Add(rb);
            }

            //not including the grandchildren means the interaction with interactables is dodgy but
            //applying forces on to the ragdoll itself looks somewhat realistic
            //i could not figure out why this was occuring but i think i prefer more realistic ragdoll interactions
            CollectRigidbodiesInChildren(child);
        }

    }


    void Update()
    {
        if (ragdollActive)
        {
            RagDoll = true;            
        }
        else
        {
            RagDoll = false;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ragdollActive = !ragdollActive;
        }
    }

   
}
