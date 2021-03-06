using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScreenShakeOnStart : MonoBehaviour
{
    [Tooltip("Small values! Default max = 1.5f or so")]
    public float Impulse = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster.instance.CameraShaker != null)
        {
            GameMaster.instance.CameraShaker.AddShake(Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
