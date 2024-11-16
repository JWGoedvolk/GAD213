using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractorHandler : MonoBehaviour
{
    [Header("On Interact")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private bool isTriggering = false;
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private UnityEvent onInteract;

    [Header("Trigger Events")]
    public List<string> BlacklistTags = new List<string>();
    public UnityEvent TriggerEnter;
    public UnityEvent TriggerExit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggering && !isTriggered && Input.GetKeyDown(interactKey))
        {
            isTriggered = true;
            if (onInteract != null)
            {
                onInteract?.Invoke();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (BlacklistTags.Contains(collision.tag)) return; // Only invoke things if they don't have an unwanted tag
        
        isTriggering = true;
        TriggerEnter?.Invoke();
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (BlacklistTags.Contains(collision.tag)) return; // Only invoke things if they don't have an unwanted tag

        isTriggering = false;
        TriggerExit?.Invoke();
    }

    public void Triggered()
    {
        isTriggered = false;
    }
}
