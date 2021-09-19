using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    //Moves paddle and tracks relevant paddle data
    //Attached to paddle gameobject in scene

    Rigidbody2D rb;
    //Serialized variables assined in engine editor
    [SerializeField]
    float speed;
    [SerializeField]
    Transform caughtTransform;

    GameController gc;
    Vector2 travelDir = Vector2.right;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Move paddle in other direction when it makes contact with a wall
        if(collision.gameObject.TryGetComponent(out Wall w))
        {
            travelDir *= -1;
            MovePaddle(travelDir);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
    }
    private void Start()
    {
        MovePaddle(travelDir);
    }

    //Called to change paddle's direction
    void MovePaddle(Vector2 dir)
    {
        rb.velocity = dir * speed;
    }

    //Called to get position to place player on paddle
    public Vector2 GetCaughtPosition()
    {
        return caughtTransform.position;
    }
}
