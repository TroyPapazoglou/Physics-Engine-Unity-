using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class PhysicsBuilder : MonoBehaviour
{

    [Tooltip("a prefab that we clone several times as children of this object")]
    public PhysicsBuilderPart Prefab;
    [Tooltip("How many prefabs to clone")]
    public int Count;
    [Tooltip("Offset in local space of this object for positioning each child")]
    public Vector3 Offset;

    public bool fixStart;
    public bool fixEnd;


    [ContextMenu("Build")]
    void Build()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            DestroyObj(rb.gameObject);

        if (Prefab == null)
            return;

        PhysicsBuilderPart previous = null;
        for (int i = 0; i < Count; i++)
        {
            PhysicsBuilderPart instance = Instantiate(Prefab, transform);

            instance.transform.SetLocalPositionAndRotation(i * Offset, Prefab.transform.localRotation);
            instance.name = name + "_" + i;

            Rigidbody rb = instance.GetComponent<Rigidbody>();

            rb.isKinematic = (i == 0 && fixStart) || (i == Count - 1 && fixEnd);

            if (previous)
            {
                foreach(Joint joint in previous.forwardJoints)
                {
                    joint.connectedBody = rb;
                }
            }
            previous = instance;
        }
    }

    void DestroyObj(Object obj)
    {
        if (Application.isPlaying)
        {
            Destroy(obj);
        }
        else
        {
            DestroyImmediate(obj);
        }
    }
}


