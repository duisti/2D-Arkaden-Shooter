using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expire : MonoBehaviour
{

    public float Duration = 1f;
    [Tooltip("Expire in specific amount of frames? 0 = disabled and use Duration instead")]
    public int Frames = 0;
    int currentFrames = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Frames != 0)
        {
            if (currentFrames == Frames)
            {
                Destroy(this.gameObject);
                return;
            }
            currentFrames++;
            return;
        }
        if (Duration <= 0f)
        {
            Destroy(this.gameObject);
            return;
        }
        Duration -= Time.deltaTime;
    }
}
