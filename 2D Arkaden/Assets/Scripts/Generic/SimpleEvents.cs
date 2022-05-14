using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SimpleEvents : MonoBehaviour
{
    [System.Serializable]
    public class RunOnStartup : UnityEvent { }

    [SerializeField]
    public RunOnStartup OnStartup;
    // Start is called before the first frame update
    void Start()
    {
        OnStartup.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
