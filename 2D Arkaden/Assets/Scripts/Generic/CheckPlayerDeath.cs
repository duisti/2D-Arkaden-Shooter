using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CheckPlayerDeath : MonoBehaviour
{
    [System.Serializable]
    public class OnPlayerDeath : UnityEvent { }
    [SerializeField]
    public OnPlayerDeath onPlayerDeath;

    bool hasRan = false;

    public float DeathTimer = 3f;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.instance == null || hasRan)
        {
            return;
        }
        if (playerStats == null && GameMaster.instance.CurrentPlayerObject != null)
        {
            playerStats = GameMaster.instance.CurrentPlayerObject.GetComponent<PlayerStats>();
        }
        if (playerStats == null)
        {
            return;
        }
        if (playerStats.GetDeathStatus())
        {
            if (DeathTimer <= 0f)
            {
                hasRan = true;
                onPlayerDeath.Invoke();
            }
            DeathTimer -= Time.deltaTime;
        }
    }
}
