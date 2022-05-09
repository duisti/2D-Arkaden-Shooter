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

    Vector2 VJoyStickInput;

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

    public void SetVJoyStickInput(float x, float y)
    {
        VJoyStickInput = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
#endif
        if (VJoyStickInput == Vector2.zero)
        {
            XInput = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
            YInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f);
        } else
        {
            XInput = Mathf.Clamp(VJoyStickInput.x, -1f, 1f);
            YInput = Mathf.Clamp(VJoyStickInput.y, -1f, 1f);
        }
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Fire3 = Input.GetButton("Fire3");
    }
}
