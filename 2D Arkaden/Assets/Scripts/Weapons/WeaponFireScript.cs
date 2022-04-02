using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireScript : MonoBehaviour
{
    public GameObject Prefab;
    public PlayerStats Stats;
    public float Cooldown = 1f;
    float cooldownTimer;

    public float HeatGenerated = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (Prefab == null) {
            Debug.LogError("Prefab not set! (WeaponFireScript.cs");
            return;

        } 
        if (Stats == null)
        {
            Stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
            if (Stats == null) return;
        }
    }
    void FireWeapon()
    {
        Stats.ModifyHeat(HeatGenerated);
        cooldownTimer = Cooldown;
        Instantiate(Prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Prefab == null) return;
        if (Stats == null) return;
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
        if (ControlManager.instance.Fire1 && cooldownTimer <= 0)
        {
            FireWeapon();
        }
    }
}