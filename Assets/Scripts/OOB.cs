using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OOB : MonoBehaviour
{
    [SerializeField]
    private bool timerRunning = false;
    [SerializeField]
    private float timer = 0;
    [SerializeField]
    private float timerDuration = 5f;
    [SerializeField]
    private TextMeshProUGUI text; 

    Ragdoll ragdoll;

    private void Start()
    {
        text = FindObjectOfType<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ragdoll = other.GetComponent<Ragdoll>();
            if (ragdoll != null && !ragdoll.ragdollActive)
            {
                if (!timerRunning)
                {                    
                    timerRunning = true;
                    timer = timerDuration;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (timerRunning)
        {
            text.enabled = false;
            text.text = "";
            timerRunning = false;
            timer = timerDuration;
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;
            text.enabled = true;
            text.text = "Out of bounds<br> Return to play area in: " + Mathf.CeilToInt(timer);
           

            if(timer <= 0)
            {
                text.enabled = false;
                text.text = "";
                ragdoll.ragdollActive = true;
                timerRunning = false;
                timer = timerDuration;
                
            }
        }
    }
}
