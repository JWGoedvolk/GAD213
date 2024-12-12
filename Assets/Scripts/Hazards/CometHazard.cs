using System.Collections;
using UnityEngine;

public class CometHazard : MonoBehaviour
{
    public Transform TargetPoint;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 5f;

    public IEnumerator CometStart()
    {
        rb = GetComponent<Rigidbody2D>();
        while (TargetPoint == null)
        {
            yield return null;
        }
        Vector3 vectorToTarget = TargetPoint.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        rb.AddForce(vectorToTarget * speed);
        rb.rotation = angle;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 vectorToTarget = TargetPoint.transform.position - transform.position;
        //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;


        //rb.rotation = angle;

        //rb.MovePositionAndRotation(TargetPoint.transform.position, angle);
    }
}
