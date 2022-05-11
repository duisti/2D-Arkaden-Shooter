using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHealth : MonoBehaviour
{
    public float Score = 150f;
    public float Health = 15f;
    float maxHealth;
    bool dead;
    GameObject DeathPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        maxHealth = Health;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageHealth(float damage)
    {
        if (dead) return;
        Health -= damage;
        if (Health <= 0f)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        dead = true;
        if (DeathPrefab != null)
        {
            Instantiate(DeathPrefab, transform.position, Quaternion.identity);
        }
        GameMaster.instance.AddLevelScore(Score);
        Destroy(this.gameObject);
    }
}
