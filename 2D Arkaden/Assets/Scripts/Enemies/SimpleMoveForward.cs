using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveForward : MonoBehaviour
{
    public enum MoveDirection
    {
        Left, Right
    }

    public MoveDirection Direction = MoveDirection.Left; //default, as enemies come towards us
    public float Speed = 8f;
    public string SpriteRootName = "Sprite_Root";
    Transform spriteRoot;
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        if (SpriteRootName != "")
        {
            spriteRoot = transform.Find(SpriteRootName);
            if (spriteRoot != null)
            {
                originalScale = spriteRoot.localScale;
            }
        }
        if (Direction == MoveDirection.Left)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180)), 360f);
            if (spriteRoot != null) spriteRoot.localScale = new Vector3(originalScale.x, originalScale.y * -1, originalScale.z);
        }
        else if (Direction == MoveDirection.Right)
        {
            if (spriteRoot != null) spriteRoot.localScale = originalScale; //spriteRoot.localScale = new Vector3(originalScale.x, originalScale.y * -1, originalScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float calcedSpeed = Speed * Time.deltaTime;
        if (GameMaster.instance != null && GameMaster.instance.PlayersPath != null)
        {
            if (Direction == MoveDirection.Right)
            {
                calcedSpeed += GameMaster.instance.PlayersPath.currentSpeed * Time.deltaTime;
            }
        }
        Vector3 dir = (transform.right * transform.localScale.x) * calcedSpeed;
        Vector3 newPos = transform.position + dir;
        /*
        if (GameMaster.instance != null)
        {
            if (GameMaster.instance.PlayersPath != null)
            {
                newPos += new Vector3(GameMaster.instance.PlayersPath.currentSpeed, 0, 0) * Time.deltaTime;
            }
        }
        */
        /*
        if (Direction == MoveDirection.Left)
        {
            spriteRoot.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
        }
        else if (Direction == MoveDirection.Right)
        {
            spriteRoot.localScale = new Vector3(originalScale.x * 1, originalScale.y, originalScale.z);
        }
        */
        
        if (spriteRoot != null)
        {
            if (!Mathf.Approximately(newPos.x, transform.position.x))
            {
                if (newPos.x < transform.position.x)
                {
                    spriteRoot.localScale = new Vector3(originalScale.x, originalScale.y * -1, originalScale.z);
                }
                else spriteRoot.localScale = originalScale;
            }
        }
        
        transform.position = newPos;
    }
}
