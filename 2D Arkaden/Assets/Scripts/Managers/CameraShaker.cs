using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public float MaxImpulse = 2f;
    float currentImpulse = 0f;
    public float SegmentDuration = 0.667f; //so an impulse of 1f would last this long

    // Start is called before the first frame update
    void Awake()
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
        if (GameMaster.instance == null) return;
        if (currentImpulse > 0f)
        {
            ShakeCamera();
        } else
        {
            currentImpulse = 0f;
        }
    }

    [Tooltip("Small values! Default max = 1.5f or so")]
    public void AddShake(float intensity)
    {
        currentImpulse += intensity;
        currentImpulse = Mathf.Clamp(currentImpulse, 0f, MaxImpulse);
    }

    void ShakeCamera()
    {
        Vector3 rootPos =  new Vector3(GameMaster.instance.PlayersCamera.transform.parent.position.x, 
            GameMaster.instance.PlayersCamera.transform.parent.position.y, 
            GameMaster.instance.PlayersCamera.transform.position.z);
        Vector3 targetPos = rootPos;
        if (currentImpulse > 0f)
        {
            targetPos = new Vector3(rootPos.x + Random.Range(-currentImpulse, currentImpulse), 
                rootPos.y + Random.Range(-currentImpulse, currentImpulse), 
                GameMaster.instance.PlayersCamera.transform.position.z);
        }
        GameMaster.instance.PlayersCamera.transform.position = targetPos;
        currentImpulse -= Time.deltaTime / SegmentDuration;
    }
}
