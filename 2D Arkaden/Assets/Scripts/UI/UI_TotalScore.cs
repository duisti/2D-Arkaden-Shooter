using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]

public class UI_TotalScore : MonoBehaviour
{

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameMaster.instance == null) return;
        float total = GameMaster.instance.GetLevelScore() + GameMaster.instance.GetTotalScore();
        text.text = "" + total;
    }
}
