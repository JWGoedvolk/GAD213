using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractorHandler : MonoBehaviour
{
    [Header("On Interact")]
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private bool isTriggering = false;

    [Header("Trigger Events")]
    public UnityEvent TriggerEnter;
    public UnityEvent TriggerExit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggering && Input.GetKeyDown(KeyCode.E))
        {
            if (onInteract != null)
            {
                onInteract.Invoke();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        isTriggering = true;
        TriggerEnter?.Invoke();
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        isTriggering = false;
        TriggerExit?.Invoke();
    }
}
