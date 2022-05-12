using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformAddToGameMaster : MonoBehaviour
{
    public Transform CameraPlayerPivot;
    public Transform PlayerShip;
    // Start is called before the first frame update
    void Start()
    {
        if (CameraPlayerPivot == null)
        {
            Debug.LogError("Please set the CameraPlayerPivot!");
        }
        PlayerShip = GetComponentInChildren<PlayerStats>().transform;
        GameMaster.instance.PlayerScreenObject = CameraPlayerPivot.gameObject;
        GameMaster.instance.CurrentPlayerObject = PlayerShip.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
