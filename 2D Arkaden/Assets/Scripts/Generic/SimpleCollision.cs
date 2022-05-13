using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SimpleCollision : MonoBehaviour
{
    // Start is called before the first frame update
    Collider2D ourCollider;
    public float CollisionDamage = 5f;
    public float SelfMultiplier = 2f;

    SimpleHealth myHealth;

    public LayerMask CollidesWith = 128; //128 is default for player only
    List<Collider2D> alreadyContacted = new List<Collider2D>();
    void Start()
    {
        ourCollider = GetComponent<Collider2D>();
        myHealth = GetComponent<SimpleHealth>();
        //print(ourCollider.bounds.size.x + "");
    }

    // Update is called once per frame
    void Update()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, ourCollider.bounds.size.x / 2, CollidesWith);
        if (hits.Length != 0)
        {
            foreach (Collider2D contact in hits)
            {
                if (!alreadyContacted.Contains(contact))
                {
                    alreadyContacted.Add(contact);
                    //find scripts and add damage
                    PlayerStats playerhealth = contact.gameObject.GetComponent<PlayerStats>();
                    SimpleHealth simplehealth = contact.gameObject.GetComponent<SimpleHealth>();
                    //if a player
                    if (playerhealth != null)
                    {
                        playerhealth.DamageHealth(CollisionDamage);
                    }
                    //if not player, and something else
                    if (simplehealth != null)
                    {
                        simplehealth.DamageHealth(CollisionDamage);
                    }
                    //damage self, only once per contact
                    if (myHealth != null)
                    {
                        myHealth.DamageHealth(CollisionDamage * SelfMultiplier);
                    }
                }
            }
        }
    }
}
