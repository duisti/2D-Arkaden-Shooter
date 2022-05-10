using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float Damage = 1f;
    [Tooltip("0f = no radius, skip aoe specific things, if any more then apply as AoE damage")]
    public float Radius = 0f; //0f = no radius, skip aoe specific things, if any more then apply as AoE damage
    [Range(0f, 1f)]
    [Tooltip("0-1 value percentage based. 0.3 would mean 30% of the damage value is also applied to main target on top of splash. Not used if radius = 0")]
    public float SplashPercent = 0f; //0-1 value percentage based. 0.3 would mean 30% of the damage value is also applied to main target on top of splash. Not used if radius = 0
    [Tooltip("if splash, does damage fall off from center 100% to edge 0%?")]
    public bool FallOff = true; //if splash, does damage fall off from center 100% to edge 0%?
    public GameObject OnDeathPrefab; //can be an effect, or another projectile etc
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
