using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Interactables");            
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Button button = hit.rigidbody.GetComponent<Button>();                
                if (button != null)
                {
                    button.TriggerDeath();                    
                }               
            }
        }
    }
}
