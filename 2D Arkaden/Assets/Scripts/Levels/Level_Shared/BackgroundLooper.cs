using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [Tooltip("Most likely Camera&PlayerPivot.")]
    public Transform TransformToFollow;

    //public GameObject BackgroundObject; //the wallpaper
    GameObject initialBackgroundObject; //the one we already have in the scene, we rip reference values from this.
    [SerializeField]
    BoxCollider2D wallpaperSpriteSize;

    int WallpaperBuffer = 3;
    [SerializeField]
    List<GameObject> wallpaperPieces; // need only 3?

    Vector3 scaleSettings;
    float baseDepth = 30f; //adjusted by the wallpaper that was initially placed
    float wallpaperXDistance;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        if (TransformToFollow == null)
        {
            TransformToFollow = GameObject.FindGameObjectWithTag("CameraAndPlayerPivot").GetComponent<Transform>();
            if (TransformToFollow == null)
            {
                Debug.LogError("Couldn't find Transform to follow automatically! Please set the transform to move! (BackgroundLooper.cs)");
            }
        }
        initialBackgroundObject = GameObject.FindGameObjectWithTag("Wallpaper");
        if (initialBackgroundObject == null)
        {

            Debug.LogError("Couldn't find initial wallpaper! (BackgroundLooper.cs)");
        }
        wallpaperSpriteSize = initialBackgroundObject.GetComponent<BoxCollider2D>();
        scaleSettings = initialBackgroundObject.transform.localScale;
        baseDepth = initialBackgroundObject.transform.position.z;
        initialBackgroundObject.transform.parent = this.transform; //just to folder all the pieces in
        wallpaperXDistance = scaleSettings.x * wallpaperSpriteSize.size.x;

        wallpaperPieces = new List<GameObject>();
        for (int i = 0; i < WallpaperBuffer; i++)
        {
            if (i == 0)
            {
                Vector3 placement = new Vector3(-wallpaperXDistance + initialBackgroundObject.transform.position.x, initialBackgroundObject.transform.position.y, baseDepth);
                wallpaperPieces.Add(Instantiate(initialBackgroundObject, placement, Quaternion.identity, this.transform));
            }
            if (i == 1)
            {
                wallpaperPieces.Add(initialBackgroundObject);
            }
            if (i == 2)
            {
                Vector3 placement = new Vector3(wallpaperXDistance + initialBackgroundObject.transform.position.x, initialBackgroundObject.transform.position.y, baseDepth);
                wallpaperPieces.Add(Instantiate(initialBackgroundObject, placement, Quaternion.identity, this.transform));
            }
            Destroy(wallpaperPieces[i].GetComponent<BoxCollider2D>()); //destroy the debug bounds so we don't interact with it even by accident ever again
        }
    }
    void DoLoop()
    {
        Vector3 newForwardSpot = wallpaperPieces[wallpaperPieces.Count - 1].transform.position;
        Vector3 add = new Vector3(wallpaperXDistance, 0, 0);
        newForwardSpot += add;
        List<GameObject> newList = new List<GameObject>(wallpaperPieces);

        for (int i = 0; i < wallpaperPieces.Count; i++)
        {
            if (i == 0)
            {
                newList[i] = wallpaperPieces[wallpaperPieces.Count -1];
            } else newList[i] = wallpaperPieces[i - 1];
        }
        wallpaperPieces = newList;
        wallpaperPieces[wallpaperPieces.Count - 1].transform.position = newForwardSpot;
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance((Vector2)TransformToFollow.position, (Vector2)wallpaperPieces[2].transform.position) < wallpaperXDistance / 10f)
        {
            DoLoop();
        }
    }
}
