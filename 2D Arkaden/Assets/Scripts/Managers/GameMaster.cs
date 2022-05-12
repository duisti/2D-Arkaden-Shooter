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
    public PlayerPath PlayerPathScript;

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
}
