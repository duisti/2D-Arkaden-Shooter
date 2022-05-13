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
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TrackingSpeed * Time.deltaTime);
        DeactivateAfter -= Time.deltaTime;
    }
}
