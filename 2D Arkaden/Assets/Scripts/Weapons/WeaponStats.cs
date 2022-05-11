using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float Damage = 1f;
    public GameObject OnDeathPrefab; //can be an effect, or another projectile etc
    public LayerMask CollidesWith;
    [Tooltip("Is damage applies as area on effect?")]
    public bool AreaOfEffect = false;
    public float Radius = 0f; //Not used if AoE=False
    [Range(0f, 1f)]
    [Tooltip("0-1 value percentage based. 0.3 would mean 30% of the damage value is also applied to main target on top of splash. Not used if AoE=False")]
    public float SplashPercent = 0f; //0-1 value percentage based. 0.3 would mean 30% of the damage value is also applied to main target on top of splash. Not used if AoE=False
    [Tooltip("if splash, does damage fall off from center 100% to edge 0%?")]
    public bool FallOff = false; //if splash, does damage fall off from center 100% to edge 0%? Not used if AoE=False
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float GetDamage()
    {
        return Damage;
    }

    public float GetRadius()
    {
        return Radius;
    }

    public float GetSplashPercent()
    {
        return SplashPercent;
    }

    public bool GetFallOff()
    {
        return FallOff;
    }

    public GameObject GetDeathPrefab()
    {
        return OnDeathPrefab;
    }
}
