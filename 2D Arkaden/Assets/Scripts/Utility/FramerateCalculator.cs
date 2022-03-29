using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateCalculator : MonoBehaviour
{
    public static FramerateCalculator instance;

    public int Buffer = 30;
    public List<float> FramerateList = new List<float>();
    [SerializeField]
    float averageFPS;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    void Start()
    {

    }
    public float GetAverageFps(bool rounded)
    {
        if (rounded)
        {
            return Mathf.Round(averageFPS);
        }
        return averageFPS;
    }

    // Update is called once per frame
    void Update()
    {
        //IF we exceeded size of buffer, lets create a new list and copy some old values from it... This is for Debugging only!
        if (FramerateList.Count > Buffer)
        {
            var oldList = FramerateList;
            FramerateList = new List<float>();
            for (int i = 0; i < Buffer; i++)
            {
                FramerateList.Add(oldList[i]);
            }
        }
        if (FramerateList.Count < Buffer) //populate the list up to the buffer amount
        {
            FramerateList.Add(1 / Time.deltaTime);
            return;
        }
        for (int i = Buffer - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                FramerateList[i] = 1 / Time.deltaTime;
            }
            else 
            {
                FramerateList[i] = FramerateList[i - 1];
            }
        }
        float sum = 0f;
        foreach (float f in FramerateList)
        {
            sum += f;
        }
        averageFPS = sum / FramerateList.Count;
    }
}
