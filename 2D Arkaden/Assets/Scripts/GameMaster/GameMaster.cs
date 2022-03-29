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
}
