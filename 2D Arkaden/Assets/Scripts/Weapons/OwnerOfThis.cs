using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerOfThis : MonoBehaviour
{
    public GameObject Owner;
    LayerMask OwnerLayer;

    [Tooltip("Attach to owner on setup?")]
    public bool AttachToOwner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup(GameObject g)
    {
        Owner = g;
        if (Owner != null)
        {
            OwnerLayer = Owner.layer;
            if (AttachToOwner)
            {
                transform.position = Owner.transform.position;
                transform.parent = Owner.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetOwner()
    {
        return Owner;
    }
    public LayerMask GetOwnerLayer()
    {
        return OwnerLayer;
    }
}
