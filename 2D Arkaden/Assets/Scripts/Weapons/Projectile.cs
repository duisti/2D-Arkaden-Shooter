using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum CollisionDetectionType
    {
        None, Raycast, Circlecast
    }

    [Tooltip("None = Does nothing, Raycast = Line forward equal to the amount moved, Circlecast = Circle forward equal to amount moved plus radius from collider")]
    public CollisionDetectionType CollisionDetection = CollisionDetectionType.Raycast; //Raycast default default
    CircleCollider2D castRadius;
    public float Speed = 15f;

    [SerializeField]
    OwnerOfThis ownerScript;
    [SerializeField]
    WeaponStats weaponStats;
    bool initialized;

    Vector2 startPos;
    [Tooltip("False = Ignore OOB")]
    public bool outOfBounds = false;
    float outOfBoundsTimer = 5f;
    // Start is called before the first frame update
    private void Awake()
    {
        startPos = transform.position;
    }
    void Start()
    {
        Init();
        if (outOfBounds)
        {
            outOfBoundsTimer = outOfBoundsTimer / Speed;
            outOfBounds = false;
        }else
        {
            outOfBoundsTimer = 20f;
        }
        
    }

    void Init()
    {
        if (CollisionDetection == CollisionDetectionType.Circlecast)
        {
            castRadius = GetComponent<CircleCollider2D>();
            if (castRadius == null)
            {
                Debug.LogError("In order to use Circlecast please give the bullet a circle collider to determine radius!");
            }
        }
        ownerScript = GetComponent<OwnerOfThis>();
        weaponStats = ownerScript.GetOwner().GetComponent<WeaponStats>();
        initialized = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialized)
        {
            Init();
        }
        if (outOfBounds)
        {
            outOfBoundsTimer -= Time.deltaTime;
            if (outOfBoundsTimer <= 0f)
            {
                DestroyThis();
            }
        }
        Vector2 originalPos = transform.position;
        if (GameMaster.instance != null)
        {
            if (GameMaster.instance.CameraBounds != null)
            {
                //float theLargerNumber = Mathf.Max(GameMaster.instance.CameraBounds.size.x, GameMaster.instance.CameraBounds.size.y);
                //theLargerNumber = Mathf.Max(theLargerNumber, (GameMaster.instance.CameraBounds.size.x + GameMaster.instance.CameraBounds.size.y) / 2f);
                //if (Vector2.Distance(startPos, transform.position) >= theLargerNumber * 1.5f)
                //{
                //    DestroyThis();
                //}
                if (!GameMaster.instance.CameraBounds.bounds.Contains((Vector3)originalPos + new Vector3(0, 0, GameMaster.instance.CameraBounds.transform.position.z)))
                {
                    outOfBounds = true;
                }
            }
        }
        float stepDist = Speed * Time.deltaTime;
        Vector2 destinationPos = transform.position + transform.right * stepDist;
        transform.position = destinationPos;

        switch (CollisionDetection)
        {
            case CollisionDetectionType.Circlecast:
                {
                    CircleCastDetection(originalPos, destinationPos, stepDist);
                    break;
                }
            case CollisionDetectionType.Raycast:
                {
                    RayCastDetection(originalPos, destinationPos, stepDist);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void DestroyThis()
    {
        //here we would instantiate an effect if such thing exists
        //instantiate whatever
        GameObject effect = weaponStats.GetDeathPrefab();
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        //here we get rid of this projectile
        Destroy(this.gameObject);
    }

    void RayCastDetection(Vector2 origin, Vector2 destination, float amount)
    {
        var heading = destination - origin;
        var distance = heading.magnitude;
        var direction = heading / distance;

        var hit = Physics2D.Raycast(origin, direction, amount, weaponStats.CollidesWith).transform;
        if (hit != null)
        {
            var hitGo = hit.transform.gameObject;
            PlayerStats playerStats = hitGo.GetComponent<PlayerStats>();
            SimpleHealth simpleHealth = hitGo.GetComponent<SimpleHealth>();
            if (playerStats != null)
            {
                playerStats.DamageHealth(weaponStats.GetDamage());
            }
            else if (simpleHealth != null)
            {
                simpleHealth.DamageHealth(weaponStats.GetDamage());
            }
            //in the end, destroy
            DestroyThis();
        }
    }

    void CircleCastDetection(Vector2 origin, Vector2 destination, float amount)
    {
        var heading = destination - origin;
        var distance = heading.magnitude;
        var direction = heading / distance;
        var hits = Physics2D.CircleCastAll(origin, castRadius.radius, direction, amount, weaponStats.CollidesWith);
        GameObject closestTarget = null;
        if (hits.Length != 0)
        {
            var possibleTargets = new List<GameObject>();
            //add hit GO's to the list
            foreach (RaycastHit2D contact in hits)
            {
                //never count yourself in
                if (contact.transform != this.transform)
                {
                    possibleTargets.Add(contact.transform.gameObject);
                }
                //might as well determine closest target here, the one we actually "hit"
                if (!weaponStats.AreaOfEffect)
                {
                    if (closestTarget != null)
                    {
                        if (Vector2.Distance(this.transform.position, closestTarget.transform.position) > Vector2.Distance(this.transform.position, contact.transform.position))
                        {
                            closestTarget = contact.transform.gameObject;
                        }
                    } else
                    {
                        closestTarget = contact.transform.gameObject;
                    }
                }
            }

            if (!weaponStats.AreaOfEffect)
            {
                PlayerStats playerStats = closestTarget.GetComponent<PlayerStats>();
                SimpleHealth simpleHealth = closestTarget.GetComponent<SimpleHealth>();
                if (playerStats != null)
                {
                    playerStats.DamageHealth(weaponStats.GetDamage());
                }
                else if (simpleHealth != null)
                {
                    simpleHealth.DamageHealth(weaponStats.GetDamage());

                }
            } else
            {
                foreach (GameObject g in possibleTargets)
                {
                    PlayerStats playerStats = g.GetComponent<PlayerStats>();
                    SimpleHealth simpleHealth = g.GetComponent<SimpleHealth>();
                    if (playerStats != null)
                    {
                        playerStats.DamageHealth(weaponStats.GetDamage());
                    }
                    else if (simpleHealth != null)
                    {
                        simpleHealth.DamageHealth(weaponStats.GetDamage());

                    }
                }
            }
            //in the end, destroy
            DestroyThis();
        }
    }

}
