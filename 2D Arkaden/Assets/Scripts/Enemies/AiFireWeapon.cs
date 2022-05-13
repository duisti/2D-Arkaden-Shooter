using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFireWeapon : MonoBehaviour
{

    public List<GameObject> Prefabs;
    public float Cooldown = 1f;
    float cooldownTimer;

    public float RaycastDistance = 100f;

    AudioSource LoopingFireSound;
    bool toggledFire = false;
    bool attemptingToFire = false;

    [Tooltip("Assign the layers we want to raycast check")]
    public LayerMask RaycastLayers;
    [Tooltip("Names of layers that we hit which will trigger firing sequence")]
    public List<string> RaycastActivators = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if (Prefabs.Count == 0)
        {
            Debug.LogError("Prefabs not set! (WeaponFireScript.cs");
            return;

        }

        LoopingFireSound = GetComponent<AudioSource>();
    }
    void FireWeapon()
    {
        cooldownTimer = Cooldown;
        for (int i = 0; i < Prefabs.Count; i++)
        {
            GameObject g = Instantiate(Prefabs[i], transform.position, transform.rotation) as GameObject;
            OwnerOfThis script = g.GetComponent<OwnerOfThis>();
            if (script != null)
            {
                script.Setup(this.gameObject);
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
        }
        else if (!attemptingToFire && toggledFire)
        {
            toggledFire = false;
            LoopingFireSound.loop = false;
        }
    }

    bool RaycastCheck()
    {
        var hit = Physics2D.Raycast(transform.position, transform.right, RaycastDistance, RaycastLayers);
        if (hit.collider != null)
        {
            //check if viable targets found in this layer
            if (RaycastActivators.Contains(LayerMask.LayerToName(hit.collider.gameObject.layer)))
            {
                print("Hit!");
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Prefabs.Count == 0) return;
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
        //some repetition here because i got lazy (could've made a struct but cba)
        if (RaycastCheck())
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
