using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireScript : MonoBehaviour
{
    public List<GameObject> Prefabs;
    public PlayerStats Stats;
    public float Cooldown = 1f;
    float cooldownTimer;

    public float HeatGenerated = 1f;

    public string inputHotkey = "Fire1"; //Fire1, Fire2, Fire3

    AudioSource LoopingFireSound;
    bool toggledFire = false;
    bool attemptingToFire = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (Prefabs.Count == 0) {
            Debug.LogError("Prefabs not set! (WeaponFireScript.cs");
            return;

        } 
        if (Stats == null)
        {
            Stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
            if (Stats == null) return;
        }

        LoopingFireSound = GetComponent<AudioSource>();
    }
    void FireWeapon()
    {
        Stats.ModifyHeat(HeatGenerated, Cooldown);
        cooldownTimer = Cooldown;
        for (int i = 0; i < Prefabs.Count; i++){
            GameObject g = Instantiate(Prefabs[i], transform.position, Quaternion.identity) as GameObject;
            OwnerOfThis script = g.GetComponent<OwnerOfThis>();
            if (script != null)
            {
                script.Setup(Stats.gameObject);
            }
            AttachToSpawner attach = g.GetComponent<AttachToSpawner>();
            if (attach != null)
            {
                attach.Attach(this.transform);
            }
        }
    }

    void FiringSound()
    {
        if (attemptingToFire && !toggledFire)
        {
            toggledFire = true;
            LoopingFireSound.loop = true;
            LoopingFireSound.Play();
        } else if (!attemptingToFire && toggledFire)
        {
            toggledFire = false;
            LoopingFireSound.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Prefabs.Count == 0) return;
        if (Stats == null) return;
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
        //some repetition here because i got lazy (could've made a struct but cba)
        if (((inputHotkey == "Fire1" && ControlManager.instance.Fire1) ||
            (inputHotkey == "Fire2" && ControlManager.instance.Fire2) ||
            (inputHotkey == "Fire3" && ControlManager.instance.Fire3))
            && !Stats.Overheated)
        {
            attemptingToFire = true;
            if (cooldownTimer <= 0)
            {
                FireWeapon();
            }
        }
        else
        {
            attemptingToFire = false;
        }
        if (LoopingFireSound != null)
        {
            FiringSound();
        }
        
    }
}
