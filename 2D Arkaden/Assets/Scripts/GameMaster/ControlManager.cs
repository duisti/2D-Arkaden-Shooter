using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ControlManager instance;

    public float XInput;
    public float YInput;
    public bool Fire1;
    public bool Fire2;
    public bool Fire3;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
#endif
        XInput = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
        YInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f);
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Fire3 = Input.GetButton("Fire3");
    }
}
