using UnityEngine;
using System.Collections.Generic;

public class DeathBox : MonoBehaviour
{
    public List<string> TagWhitelist = new();
    public bool DestoryOnCollide = false;
    public bool DestoryOnTrigger = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (TagWhitelist.Contains(collision.gameObject.tag))
        {
            if (DestoryOnCollide)
            {
                if (collision.gameObject != null)
                {
                    Destroy(collision.gameObject); 
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TagWhitelist.Contains(collision.gameObject.tag))
        {
            if (DestoryOnTrigger)
            {
                if (collision.gameObject != null)
                { 
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
