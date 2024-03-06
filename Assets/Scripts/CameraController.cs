using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float camSpeed = 10;
    public float camOffset = 5;
    float currentDistance;
    public float distanceBack;
    public float zoomSpeed = 5;
    public float returnToOffsetSpeed = 5;
    public float heightOffset = 5;

  

    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            Vector3 angles = transform.eulerAngles;
            float dx = Input.GetAxis("Mouse Y");
            float dy = Input.GetAxis("Mouse X");
            angles.x = Mathf.Clamp(angles.x + dx * camSpeed * Time.deltaTime, 0, 70);
            angles.y += dy * camSpeed * Time.deltaTime;
            transform.eulerAngles = angles;
        }
      
        


        RaycastHit hit;
        LayerMask layerMask = ~LayerMask.GetMask("BodyColliders");
        if (Physics.Raycast(GetTargetPosition(), -transform.forward, out hit, camOffset, layerMask))
        {
            currentDistance = hit.distance;
        }
        else
        {
            currentDistance = Mathf.MoveTowards(currentDistance, camOffset, Time.deltaTime * returnToOffsetSpeed);
        }

        //TODO: scroll stuff drove me nuts come back to this or leave it out completely :P
        //distanceBack = Mathf.Clamp(distanceBack - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 2, 10);

        

        transform.position = GetTargetPosition() - currentDistance * transform.forward;
       
    }

    Vector3 GetTargetPosition()
    {
        return target.position + heightOffset * Vector3.up;
    }
}
