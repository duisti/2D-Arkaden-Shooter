using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 359.999f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
