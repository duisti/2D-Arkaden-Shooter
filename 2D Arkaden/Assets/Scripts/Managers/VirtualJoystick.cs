using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public GameObject Root;
    public Image JoyStickPivot;
    Image joystick;
    [SerializeField]
    Vector2 InputDir;

    bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        joystick = JoyStickPivot.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (JoyStickPivot == null)
        {
            return;
        }
        // if we are on mobile or editor display virtual joystick, else hide it (PC build) and do not proceed
        if (Application.isMobilePlatform || Application.isEditor)
        {
            Root.SetActive(true);
            isEnabled = true;
        } else
        {
            Root.SetActive(false);
            isEnabled = false;
            return;
        }
    }

    void RelayData(float x, float y)
    {
        GameMaster.instance.ControlManager.SetVJoyStickInput(x, y);
    }

    //need to copy-paste'ish because unity's event system can only take 1 arg through the editor
    public void FireButton1(bool pressed) //"Fire1, Fire2, Fire3"
    {
        GameMaster.instance.ControlManager.ButtonPush(pressed, "Fire1");
    }
    public void FireButton2(bool pressed) //"Fire1, Fire2, Fire3"
    {
        GameMaster.instance.ControlManager.ButtonPush(pressed, "Fire2");
    }
    public void FireButton3(bool pressed) //"Fire1, Fire2, Fire3"
    {
        GameMaster.instance.ControlManager.ButtonPush(pressed, "Fire3");
    }

    public void OnDrag(PointerEventData data)
    {
        if (!isEnabled) return;

        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(JoyStickPivot.rectTransform, data.position, data.pressEventCamera, out pos);
        print(JoyStickPivot.rectTransform.sizeDelta.x);
        pos.x = (pos.x / JoyStickPivot.rectTransform.sizeDelta.x);
        pos.y = (pos.y / JoyStickPivot.rectTransform.sizeDelta.y);

        //multiply by 2 since we need to calculate width/height of the deadzone
        float x = (JoyStickPivot.rectTransform.pivot.x == 1f) ? pos.x * 2 : pos.x * 2;
        float y = (JoyStickPivot.rectTransform.pivot.y == 1f) ? pos.y * 2 : pos.y * 2;

        InputDir = new Vector2(x, y);
        InputDir = (InputDir.magnitude > 1) ? InputDir.normalized : InputDir;
        RelayData(InputDir.x, InputDir.y);
        joystick.rectTransform.anchoredPosition = new Vector2(InputDir.x * (JoyStickPivot.rectTransform.sizeDelta.x / 3) , InputDir.y * (JoyStickPivot.rectTransform.sizeDelta.y) / 3);
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!isEnabled) return;
        OnDrag(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (!isEnabled) return;
        //zero out the joystick on release
        InputDir = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        RelayData(InputDir.x, InputDir.y);
    }
}
