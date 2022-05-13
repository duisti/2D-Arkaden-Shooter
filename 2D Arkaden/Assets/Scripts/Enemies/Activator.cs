using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [Tooltip("If enabled, the below value will be used and is not automatically set. Disabled, we will set it based on camera bounds")]
    public bool UseCustomActivatorDistance;
    public float ActivatorDistance = 8.75f;
    BoxCollider2D CameraBounds;

    bool initialized = false;
    bool activated = false;
    bool deactivated = false;

    public float ActivatorDeactivateMultiplier = 10f;

    public bool zeroTheY = true;

    List<GameObject> childrenGameObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (initialized || GameMaster.instance == null) return;
        if (zeroTheY)
        {
            float yValue = transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
            foreach (Transform child in transform)
            {
                Vector3 pos = child.transform.position;
                pos = new Vector3(child.transform.position.x, yValue + child.transform.position.y, 0);
                child.transform.position = pos;
            }
        }
        foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
            if (t != this.transform)
            {
                childrenGameObjects.Add(t.gameObject);
                t.gameObject.SetActive(false);
            }
        }
        if (!UseCustomActivatorDistance)
        {
            GameObject MainCameraObject = null;
            FitBoundsToCamera script = null;
            print("1");
            if (GameMaster.instance.PlayerScreenObject != null)
            {
                MainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
                script = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<FitBoundsToCamera>();
            }
            else return;
            print("2");
            if (script != null)
            {
                CameraBounds = script.Bounds;
            }
            else return;
            ActivatorDistance = CameraBounds.size.x / 2f;
            print("3");
        }
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            Init();
            return;
        }
        //first check if we are almost at the screen bounds
        if (Vector3.Distance(this.transform.position, GameMaster.instance.PlayerScreenObject.transform.position) <= ActivatorDistance && !activated)
        {
            activated = true;
            foreach (GameObject g in childrenGameObjects)
            {
                if (g != null)
                {
                    g.SetActive(true);
                }
            }
        }
        //then disable when we are very far from the player (3x distance or so from original value), as we go offscreen
        if (Vector3.Distance(this.transform.position, GameMaster.instance.PlayerScreenObject.transform.position) >= ActivatorDistance * ActivatorDeactivateMultiplier && activated && !deactivated)
        {
            deactivated = true; 
            foreach (GameObject g in childrenGameObjects)
            {
                if (g != null)
                {
                    g.SetActive(false);
                }
            }
        }
    }
}
