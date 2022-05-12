using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour
{
    public float ActivateAfterSeconds = 5f;
    public float TrackingSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivateAfterSeconds > 0f)
        {
            ActivateAfterSeconds -= Time.deltaTime;
            return;
        }

        var target = GameMaster.instance.CurrentPlayerObject.transform.position;
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TrackingSpeed * Time.deltaTime);
    }
}
