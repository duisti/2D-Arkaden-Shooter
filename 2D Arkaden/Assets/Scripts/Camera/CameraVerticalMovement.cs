using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVerticalMovement : MonoBehaviour
{

    Transform playerTransform;
    Transform cameraPivot;
    [Tooltip("The maximum amount the camera can adjust up/down from its Y LocalTransform.")]
    public float MaxVerticalDistance = 0f;
    [Tooltip("Approximate time the camera needs to arrive at destination.")]
    public float SmoothTime = 0.25f;
    [Tooltip("how fast the camera moves at maximum. 5f = 5 units per second.")]
    public float MaxCameraMovementSpeed = 5.5f;
    float yVelocity = 0f;
    // Start is called before the first frame update
    void Start()
    {
        FindPlayerTransform();
        if (cameraPivot == null)
        {
            FindPivotParent();
        }
    }

    void FindPlayerTransform()
    {
        if (GameMaster.instance.CurrentPlayerObject != null)
        {
            playerTransform = GameMaster.instance.CurrentPlayerObject.GetComponent<Transform>();
        }
        else
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    void FindPivotParent()
    {
        if (transform.parent.tag == ("CameraAndPlayerPivot") || transform.parent.name.Contains("Pivot"))
        {
            cameraPivot = transform.parent;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraPivot == null)
        {
            FindPivotParent();
            return;
        }
        if (MaxVerticalDistance == 0)
        {
            return;
        }
        float playerY = playerTransform.localPosition.y;
        float cameraY = transform.localPosition.y;
        playerY = Mathf.Clamp(playerY, -MaxVerticalDistance, MaxVerticalDistance);
        //cameraY = Mathf.MoveTowards(cameraY, playerY, Mathf.Abs(Mathf.Lerp(playerY , cameraY, CameraMovementSpeed * Time.deltaTime)));
        cameraY = Mathf.SmoothDamp(cameraY, playerY, ref yVelocity, SmoothTime, MaxCameraMovementSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, cameraY, transform.localPosition.z);
    }
}
