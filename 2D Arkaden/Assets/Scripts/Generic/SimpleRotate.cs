using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [Tooltip("Degrees per second")]
    public float speed = 30f;

    [Tooltip("Enter two values and the speed will be randomized between these numbers. Must be size = 2 to be used")]
    public List<float> SpeedRange = new List<float>();

    public float startAfter = 0f;
    public float stopAfter = 0f;
    bool isTimed = false;
    // Start is called before the first frame update
    void Start()
    {
        if (startAfter != 0f)
        {
            isTimed = true;
        }
        if (SpeedRange.Count == 2)
        {
            speed = Random.Range(SpeedRange[0], SpeedRange[1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimed && startAfter > 0f)
        {
            startAfter -= Time.deltaTime;
            return;
        }
        if (isTimed && startAfter <= 0f && stopAfter <= 0f)
        {
            return;
        }
        float currentRot = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentRot + speed*Time.deltaTime));
        if (isTimed)
        {
            stopAfter -= Time.deltaTime;
        }
    }
}
