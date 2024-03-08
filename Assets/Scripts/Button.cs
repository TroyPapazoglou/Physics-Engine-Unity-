using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Ragdoll ragdoll;
    public float explosionForce;

    public void TriggerDeath()
    {
        ragdoll = FindObjectOfType<Ragdoll>();
        if(ragdoll != null)
        {
            ragdoll.ragdollActive = true;            
        }
    }
}
