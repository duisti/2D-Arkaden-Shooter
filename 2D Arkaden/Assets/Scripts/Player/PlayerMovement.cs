using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerStats))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float MaxSpeed = 5f; //set these in PlayerStats.cs
    [SerializeField]
    float Acceleration = 30f;
    //public float Decceleration = 7f; // commenting out to keep it simple. Accel will also be used for deccel.
    [SerializeField]
    Vector2 lastVelocity; // save previous velocity / direction

    [HideInInspector] //hides the values in inspector (public variables show up on inspector, reduce clutter)
    public float XInput;
    [HideInInspector]
    public float YInput; //we don't apply input directly to the values we want, instead we read them from these variables. Reason: Multiple control possibilities
                         //(virtual joystick, testing with keyboard etc..)
    BoxCollider2D CameraBounds;

    [HideInInspector]
    [Tooltip("How far do we track last positions in to frames? Has no effect on actual collision reverse calculations, just assigns the buffer!")]
    public int PositionsBuffer = 3;
    [HideInInspector]
    public List<Vector2> LastPositions = new List<Vector2>();
    [SerializeField]
    [Tooltip("What layers are considered for the collision check?")]
    LayerMask layerCheck;
    PlayerStats stats;
    private void Awake()
    {
        if (stats == null)
        {
            stats = GetComponent<PlayerStats>();
        }
        if (stats == null)
        {
            Debug.LogError("Somehow stats still not found! (PlayerMovement.cs");
        }
        MaxSpeed = stats.MaxSpeed;
        Acceleration = stats.Acceleration;
    }
    void Start()
    {
        if (CameraBounds == null)
        {
            FindCameraBounds();
        }
        PositionBuffer();
    }

    void FindCameraBounds()
    {
        CameraBounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
    }

    void DEBUG_ReadInput()
    {
        XInput = ControlManager.instance.XInput;
        YInput = ControlManager.instance.YInput;
    }
    void Update()
    {
        if (CameraBounds == null)
        {
            FindCameraBounds();
        }
        //debug, editor mode only, read keyboard input
#if UNITY_EDITOR
        DEBUG_ReadInput();
#endif
        //When XInput and YInput has been set, apply values
        ApplyValues();
        //track last positions
        PositionBuffer();
    }

    void PositionBuffer()
    {
        if (LastPositions.Count > PositionsBuffer)
        {
            var oldList = LastPositions;
            LastPositions = new List<Vector2>();
            for (int i = 0; i < PositionsBuffer; i++)
            {
                LastPositions.Add(oldList[i]);
            }
        }
        if (LastPositions.Count < PositionsBuffer) //populate the list up to the buffer amount
        {
            for (int i = 0; i < PositionsBuffer; i++)
            {
                LastPositions.Add(transform.position);
            }
            return;
        }
        for (int i = PositionsBuffer - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                LastPositions[i] = transform.position;
            }
            else
            {
                LastPositions[i] = LastPositions[i - 1];
            }
        }
    }

    void ApplyValues()
    {
        CheckCollision(); //check first. We want to be inside the stuff already before doing the damage and push
        Vector2 newVelocity = lastVelocity;
        float applyX;
        float applyY;
        //math from inputs and accel/decel
        //so when we multiply MaxSpeed with Input, not only this allows half values (for example joystick not completely tilted), it also allows negatives
        applyX = Mathf.MoveTowards(newVelocity.x, MaxSpeed * XInput, Acceleration * Time.deltaTime);
        applyY = Mathf.MoveTowards(newVelocity.y, MaxSpeed * YInput, Acceleration * Time.deltaTime);

        newVelocity = new Vector2(applyX, applyY);
        Vector2 newCoords = this.transform.position += (Vector3)newVelocity * Time.deltaTime;
        //clamp x and y inside camerabounds
        newCoords.x = Mathf.Clamp(newCoords.x, -CameraBounds.bounds.extents.x + CameraBounds.transform.position.x, CameraBounds.bounds.extents.x + CameraBounds.transform.position.x);
        newCoords.y = Mathf.Clamp(newCoords.y, -CameraBounds.bounds.extents.y + CameraBounds.transform.position.y, CameraBounds.bounds.extents.y + CameraBounds.transform.position.y);
        //this.transform.localPosition += (Vector3)newVelocity * Time.deltaTime;
        this.transform.position = newCoords;
        lastVelocity = newVelocity;
    }

    Vector2 CollisionReflection(Vector2 closestPoint, Vector2 _velocity)
    {
        Vector2 BounceVelocity = Vector2.zero;
        float ourSpeed = Mathf.Min(Mathf.Abs(_velocity.x) + Mathf.Abs(_velocity.y), MaxSpeed);
        var heading = (Vector2)transform.position - closestPoint;
        var distance = heading.magnitude;
        var direction = heading / distance;
        BounceVelocity = direction * ourSpeed;
        return BounceVelocity;
    }

    void CheckCollision() //false = no collision, we only check terrain/objects collision, enemy collisions have their own checks
    {
        //get a list of colliders matching our layermask, and handle the closest contact point only
        var hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius, Vector2.zero, 0, layerCheck);
        if (hits.Length != 0)
        {
            Vector2 closestPoint = hits[0].point;
            var contactCollider = hits[0].collider; //save this for later, when we do the backup checks if we got knocked off the map
            foreach (RaycastHit2D contact in hits)
            {
                if (Vector2.Distance(contact.point, transform.position) < Vector2.Distance(closestPoint, transform.position)) //check if distance between is shorter than presumed #1 in the list
                {
                    closestPoint = contact.point;
                    contactCollider = contact.collider;
                }
            }
            //Debug.LogError("Paused! Check!");
            //damage the player on collision
            //Collision damage equals CollisionDamage(turned in to per second) plus (Damage*LerpedVelocity) being applied to our craft, so we multiply with deltatime
            //lerping, because we don't want faster planes to be able to take more collision damage on hit. We determine collision damage from other statistics instead.
            //which means if we idle in to a wall, we won't insta explode.
            //so the end result is roughly, for ex 50 value = 50dps when afking in to a wall, or 550dps(in fact roughly if we ram full speed in to a wall, its just one collision of 50 damage)
            //multiplier can't exceed max speed.
            stats.DamageHealth(stats.CollisionDamage * Time.deltaTime + (stats.CollisionDamage * (Mathf.Lerp(0, 1, Mathf.Min(Mathf.Abs(lastVelocity.magnitude), MaxSpeed) / MaxSpeed))));

            //we want to flip our velocity to bounce us back safely, so we need to do some determinations on how we collided.
            //So we get a bounce-back like as if you'd throw a tennis ball towards the ground
            //we'll apply some velocity towards the direction we want to be pushed towards

            //Overall, my brain not enough to make this system perfect. But it's good enough now.
            Vector2 newVelocity = CollisionReflection(closestPoint, lastVelocity);
            
            //move to a previous position one frame away to ensure we dont get owned
            //we still get owned if we idle and hit an object, and we take some damage over time
            transform.position = LastPositions[1];
            //if this new position is out of camera bounds, we die IMMEDIATELY. So we dont just clip through objects if we get trapped!
            if (transform.position.x > -CameraBounds.bounds.extents.x + CameraBounds.transform.position.x && transform.position.x < CameraBounds.bounds.extents.x + CameraBounds.transform.position.x
                || transform.position.y > -CameraBounds.bounds.extents.y + CameraBounds.transform.position.y && transform.position.y < CameraBounds.bounds.extents.y + CameraBounds.transform.position.y)
            {
                //inside range, another check if we are still inside the collider as a backup
                //the circle radius divisor is there to give some forgiveness (a lot of it) if player is about to get wiped, but could slip from a corner
                var newHits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius / 100, Vector2.zero, 0, layerCheck);
                if (newHits.Length != 0)
                {
                    foreach (RaycastHit2D contact in hits)
                    {
                        if (contact.collider == contactCollider) 
                        {
                            //Woops! We die now
                            stats.DamageHealth(stats.Health);
                        }
                    }
                }
            }
            else
            {
                //outside range, death by huge numbers
                stats.DamageHealth(stats.Health);
            }

            //finally, apply the new velocity so when we run movement calcs again, we have it to this and we just dont bounce back... (of course requires player effort to stop pushing in to that dir)
            lastVelocity = newVelocity;
        }
    }
}


