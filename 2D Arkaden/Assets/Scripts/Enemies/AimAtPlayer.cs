using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour
{
    public float ActivateAfterSeconds = 5f;
    public float TrackingSpeed = 50f;

    [Tooltip("After activation, deactivates after this seconds. Disabled if 0")]
    public float DeactivateAfter = 0f;
    bool haveToDeactivate = false;

    [Tooltip("Leave empty if you don't want angle limits. Draw a collider with Angle_Test layer if you want limits")]
    public Collider2D AngleLimit;
    // Start is called before the first frame update
    void Start()
    {
        if (DeactivateAfter > 0)
        {
            haveToDeactivate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivateAfterSeconds > 0f)
        {
            ActivateAfterSeconds -= Time.deltaTime;
            return;
        }

        if (haveToDeactivate && DeactivateAfter <= 0f)
        {
            return;
        }

        var target = GameMaster.instance.CurrentPlayerObject.transform.position;
        
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion oldRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Quaternion setRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TrackingSpeed * Time.deltaTime);
        transform.rotation = setRotation;
        if (AngleLimit != null)
        {
            bool missedAngleLimit = true;
            int layerMask = LayerMask.GetMask("Angle_Test");
            var hits = Physics2D.RaycastAll(transform.position, transform.right, 5f, layerMask);
            //Gizmos.color = Color.red;
            //Gizmos.DrawRay(transform.position, transform.right);
            if (hits.Length != 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject == AngleLimit.gameObject)
                    {
                        missedAngleLimit = false;
                        print("Hit: " + hit.collider.gameObject);
                    }
                }
            }
            if (missedAngleLimit)
            {
                print("no hits, reverting");
                transform.rotation = oldRotation;
            }
        }
        
        DeactivateAfter -= Time.deltaTime;
    }
}
