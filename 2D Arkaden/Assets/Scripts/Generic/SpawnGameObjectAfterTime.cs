using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObjectAfterTime : MonoBehaviour
{

    public float Timer = 0f;
    public GameObject GameObject;
    // Start is called before the first frame update
    void Start()
    {
        Check();
    }

    void Check()
    {
        if (Timer <= 0f)
        {
            if (GameObject != null)
            {
                Instantiate(GameObject, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Check();
        Timer -= Time.deltaTime;
    }
}
