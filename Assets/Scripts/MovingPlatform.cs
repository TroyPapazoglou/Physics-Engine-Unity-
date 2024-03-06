using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private float leftPos;
    [SerializeField] private float rightPos;
    private float TimeToMove = 3f;
    private bool StartLeft = true;

    Rigidbody rb;
    CharacterController cc;
  

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(Move());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cc = other.gameObject.GetComponent<CharacterController>();           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cc.Move(rb.velocity * Time.fixedDeltaTime);           
        }
    }

    private IEnumerator Move()
    {
        float t = 0f;

        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition;

            if (StartLeft)
            {
                targetPosition = new Vector3(leftPos, transform.position.y, transform.position.z);
            }
            else
            {
                targetPosition = new Vector3(rightPos, transform.position.y, transform.position.z);
            }

            while (t < 1f)
            {
                t += Time.deltaTime / TimeToMove;               

                Vector3 direction = (targetPosition - startPosition).normalized;
                float distance = Vector3.Distance(startPosition, targetPosition);
                float speed = distance / TimeToMove;

                rb.velocity = direction * speed;

                yield return null;
            }

            StartLeft = !StartLeft;
            t = 0f;
        }
    }
}
