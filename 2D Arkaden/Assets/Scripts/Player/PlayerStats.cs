using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //health, heat & damage modifiers
    public float MaxHealth = 100f; // we never want to go over this, so most likely will be your current health. Keep it above zero in editor so we don't insta die
    public float Health = 100f;

    public float MaxHeat = 100f;
    public float Heat = 0f;
    public float HeatDissipation = 20f; // per second
    public bool Overheated = false;

    public float CollisionDamage = 7.5f; //default, actually controlled & overridden from GameMaster + modifiers

    //mobility stats
    public float MaxSpeed = 5f;
    public float Acceleration = 30f;

    float heatStopTimer = 0f;

    //equipment stuff

    [SerializeField]
    bool dead = false; // below zero = dead
    public List<GameObject> DeathPrefabs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //here we could read values from some master gameobject to set values, if we want to
        //CollisionDamage = GameMaster.instance.CollisionDamageBase;
    }

    public void SetMaxHealth(float value)
    {
        MaxHealth = value;
    }
    public void SetHealth(float value)
    {
        Health = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) //if we dead then stop tracking
        {
            return;
        }
        //safety measure, usually damage alone should kill us
        dead = CheckDeath();
        HandleHeat();
    }

    void HandleHeat()
    {
        if (Overheated || Heat >= MaxHeat)
        {
            Overheated = true;
            if (Heat <= MaxHeat / 2)
            {
                Overheated = false;
            }
        }
        if (heatStopTimer > 0)
        {
            heatStopTimer -= Time.deltaTime;
        }
        else
        {
            Heat = Mathf.Max(0, Heat - Time.deltaTime * HeatDissipation);
        }
    }

    void DoDeath()
    {
        gameObject.SetActive(false); //for now just delete character by disabling it
        if (DeathPrefabs.Count != 0)
        {
            foreach(GameObject g in DeathPrefabs)
            {
                Instantiate(g, transform.position, Quaternion.identity);
            }
        }
        return;
    }

    bool CheckDeath()
    {
        if (Health <= Mathf.Epsilon) // epsilon = 0.000000000000000000000000000000000000000000000000000000001f
        {
            DoDeath();
            return true;
        }
        return false;
    }

    //we want separate functions, in case we do effects based on the outcome
    public void DamageHealth(float amount)
    {
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        CheckDeath();
    }

    public void HealHealth(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
    }

    public void ModifyHeat(float amount, float stopTime)
    {
        heatStopTimer = stopTime + 0.25f; // ideally weapon cooldown plus .25 seconds
        Heat = Mathf.Max(0, Heat + amount);
    }

    public bool GetDeathStatus()
    {
        return dead;
    }
}
