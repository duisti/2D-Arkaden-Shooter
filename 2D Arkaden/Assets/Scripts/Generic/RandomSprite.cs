using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        if (Sprites.Count != 0)
        {
            int randomInt = Random.Range(0, Sprites.Count);
            GetComponent<SpriteRenderer>().sprite = Sprites[randomInt];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
