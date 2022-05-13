using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    //Player Vehicle Variables
    public float PlayerMaxHealthBase = 100f;
    public float PlayerCurrentHealthBase = 100f;

    [Tooltip("Damage per second when colliding, plus some math based on player's own velocity")]
    public float CollisionDamageBase = 5f;

    public GameObject CurrentPlayerObject;
    public GameObject PlayerScreenObject;
    [HideInInspector]
    public GameObject PlayersCamera;
    [HideInInspector]
    public PlayerPath PlayerPathScript;
    [HideInInspector]
    public BoxCollider2D CameraBounds;
    [HideInInspector]
    public PlayerPath PlayersPath;

    float currentLevelScore = 0f;
    float savedLevelScore = 0f;

    float totalScore = 0f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayersCamera == null)
        {
            PlayersCamera = GameObject.FindGameObjectWithTag("MainCamera");
            CameraBounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
        }
    }

    public void SaveLevelScore()
    {
        savedLevelScore = currentLevelScore;
    }

    public void ResetLevelScore()
    {
        currentLevelScore = 0f;
        savedLevelScore = 0f;
    }

    public float GetLevelScore()
    {
        return currentLevelScore;
    }

    public void AddLevelScore(float amount)
    {
        currentLevelScore += amount;
    }

    public void SetLevelScore(float amount)
    {
        currentLevelScore = amount;
    }

    public void SaveTotalScore()
    {
        totalScore += currentLevelScore;
    }

    public float GetTotalScore()
    {
        return totalScore;
    }

    public bool IsOutOfBounds(Vector3 point)
    {
        if (CameraBounds != null)
        {
            //float theLargerNumber = Mathf.Max(GameMaster.instance.CameraBounds.size.x, GameMaster.instance.CameraBounds.size.y);
            //theLargerNumber = Mathf.Max(theLargerNumber, (GameMaster.instance.CameraBounds.size.x + GameMaster.instance.CameraBounds.size.y) / 2f);
            //if (Vector2.Distance(startPos, transform.position) >= theLargerNumber * 1.5f)
            //{
            //    DestroyThis();
            //}
            if (CameraBounds.bounds.Contains((Vector3)point + new Vector3(0, 0, CameraBounds.transform.position.z)))
            {
                return false;
            }
        }
        else return false;
        return true;
    }
}
