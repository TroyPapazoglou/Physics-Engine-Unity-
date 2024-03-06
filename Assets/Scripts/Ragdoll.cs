using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ragdoll : MonoBehaviour
{
    private Animator animator = null;
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
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
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        foreach(Rigidbody r in rigidbodies)
        {
            r.isKinematic = true;
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
    }

}
