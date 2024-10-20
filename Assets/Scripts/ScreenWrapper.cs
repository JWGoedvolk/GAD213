using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    public Vector2 width;
    public Vector2 height;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < width.x) { transform.position = new Vector3(width.y - 0.1f, transform.position.y, transform.position.z); }
        if (transform.position.x > width.y) { transform.position = new Vector3(width.x + 0.1f, transform.position.y, transform.position.z); }

        if (transform.position.y < height.x) { transform.position = new Vector3(transform.position.x, height.y, transform.position.z); }
        if (transform.position.y > height.y) { transform.position = new Vector3(transform.position.x, height.x, transform.position.z); }
    }
}
