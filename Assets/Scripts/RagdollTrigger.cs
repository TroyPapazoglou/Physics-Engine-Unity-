using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ragdoll ragdoll = other.gameObject.GetComponent<Ragdoll>();
        if (ragdoll != null)
            ragdoll.ragdollActive = true;
    }
}
