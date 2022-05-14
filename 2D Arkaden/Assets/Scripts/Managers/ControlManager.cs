using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ControlManager instance;

    int virtualButtonPushes = 0;

    public float XInput;
    public float YInput;
    public bool Fire1;
    public bool Fire2;
    public bool Fire3;

    Vector2 VJoyStickInput;

    private void Awake()
    {
        if (ControlManager.instance != null)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        ControlManager.instance = this;
    }
    void Start()
    {
        
    }

    public void SetVJoyStickInput(float x, float y)
    {
        VJoyStickInput = new Vector2(x, y);
    }

    public void ButtonPush(bool pressed, string inputName)
    {
        if (pressed)
        {
            virtualButtonPushes++;
        }
        else virtualButtonPushes = Mathf.Max(0, virtualButtonPushes - 1);
        switch (inputName)
        {
            case "Fire1":
                Fire1 = pressed;
                break;
            case "Fire2":
                Fire2 = pressed;
                break;
            case "Fire3":
                Fire3 = pressed;
                break;
            default:
                break;
        }
        print("Pressed: " + inputName + ", " + virtualButtonPushes);
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
        if (virtualButtonPushes == 0)
        {
            Fire1 = Input.GetButton("Fire1");
            Fire2 = Input.GetButton("Fire2");
            Fire3 = Input.GetButton("Fire3");
        }
    }
}
