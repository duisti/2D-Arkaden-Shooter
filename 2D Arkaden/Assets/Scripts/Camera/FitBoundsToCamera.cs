using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class FitBoundsToCamera : MonoBehaviour
{
    [Tooltip("Gets auto assigned if null")]
    public BoxCollider2D Bounds; //Defaults: x - 17.78337 , y - 10, presuming orthographic size is 5
    Camera AttachedCamera; //usually the parent, actually we presume to be so
    CircleCollider2D PlayersCollider; // adjust based on the player collider, so it cant half clip outside of map
    // Start is called before the first frame update
    private void Awake()
    {
        if (Bounds == null)
        {
            Bounds = this.GetComponent<BoxCollider2D>();
        }
        AttachedCamera = this.transform.parent.GetComponent<Camera>();
        //move the bounds up one hierarchy, since otherwise we screw up camera shake
        this.transform.parent = AttachedCamera.transform.parent;
    }
    void Start()
    {
        //lets try to find the players collider
        //if we can't find it from GameMaster, lets try to find via tag.
        //if that doesn't work, then ignore player's collider for bounds adjustments
        FindPlayerCollider();
    }

    void FindPlayerCollider()
    {
        if (GameMaster.instance.CurrentPlayerObject != null)
        {
            PlayersCollider = GameMaster.instance.CurrentPlayerObject.GetComponent<CircleCollider2D>();
        }
        else
        {
            PlayersCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //we want to constantly fix the bounds to the camera, in case we have zoom in/out events!
        float playerSizeAdjustment = 0f;
        if (PlayersCollider != null)
        { 
            //adjust. Based on transform size, since if we have scale up/down powerups, they will modify localscale instead of collider radius (collider radius gets scaled up, but values of collier stay same).
            playerSizeAdjustment = PlayersCollider.radius * ((PlayersCollider.transform.localScale.x + PlayersCollider.transform.localScale.y) / 2);
        } else
        {
            FindPlayerCollider();
        }
        float ySize = AttachedCamera.orthographicSize * 2;
        float xSize = ySize * Screen.width / Screen.height;
        Bounds.size = new Vector2(xSize - playerSizeAdjustment, ySize - playerSizeAdjustment);
    }
}
