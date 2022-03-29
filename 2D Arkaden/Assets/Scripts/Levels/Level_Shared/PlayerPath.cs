using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
    [SerializeField][Tooltip("This list gets populated based on children. Make sure they are in correct order! (From top to bottom)")]
    List<Transform> WayPoints;
    public Transform TransformToMove; // remember to set this..!
    [SerializeField]
    Transform nextPoint;
    int travelCount = 0; //to track position in list
    float currentSpeed = 0;
    [SerializeField]
    float targetSpeed = 3;
    [SerializeField]
    float acceleration = 3;

    // Start is called before the first frame update
    void Start()
    {
        //if travelpoints is empty (designer being lazy!), try to autopopulate based on children
        if (WayPoints.Count == 0)
        {
            foreach (Transform child in transform)
            {
                WayPoints.Add(child);
            }
        }
        if (TransformToMove == null)
        {
            Debug.LogError("Please set the transform to move! (PlayerPath.cs)");
        }
        //lets move our list of waypoints in to a separate gameobject not attached to anything, because otherwise the points might keep moving as player moves...

    }

    bool GetNextPoint()
    {
        Debug.Log("nextPoint is null, looking for the next entry in TravelPoints (travelCount : " + travelCount + ")");
        nextPoint = WayPoints[travelCount];
        travelCount++;
        if (nextPoint != null) return true;
        else return false;
    }

    // Update is called once per frame
    void LateUpdate() //move the root after player movement
    {
        if (WayPoints.Count == 0) return;
        //handle finding targets if lacking requirements
        while (nextPoint == null && travelCount < WayPoints.Count)
        {
            GetNextPoint();
        }
        //movement speed calc
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        //apply movement, but do some checks first
        Vector2 currentPos = TransformToMove.position;
        Vector2 targetPos = Vector2.MoveTowards(currentPos, nextPoint.position, currentSpeed * Time.deltaTime);
        //check if positions match = we have arrived
        if (targetPos == new Vector2(nextPoint.position.x, nextPoint.position.y))
        {
            //check if we're beyond our list size
            if (travelCount < WayPoints.Count)
            {
                //if we don't find a new point (next entry is null), keep trying until we run out of list size or find something
                bool found = GetNextPoint();
                while (!found && travelCount < WayPoints.Count)
                {
                    GetNextPoint();
                }
            }
            //finally recalculate targetpos again, so we don't get choppy movement when swapping between waypoints
            targetPos = Vector2.MoveTowards(currentPos, nextPoint.position, currentSpeed * Time.deltaTime);
        }
        TransformToMove.position = targetPos;
    }

    public void SetTravelSpeed(float newTarget, float newAcceleration)
    {
        targetSpeed = newTarget;
        acceleration = newAcceleration;
    }
    public float getTargetSpeed()
    {
        return targetSpeed;
    }
    public float getAcceleration()
    {
        return acceleration;
    }
}
