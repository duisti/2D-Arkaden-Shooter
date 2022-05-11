using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attach(Transform t)
    {
        transform.position = t.transform.position;
        transform.parent = t.transform;
    }
}
