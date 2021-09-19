using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Contains methods to control a given block
    //Attached to block prefab in engine editor

    //Serialized variables assined in engine editor
    [SerializeField]
    int scoreValue;
    [SerializeField]
    float speedModValue;


    GameController gc;
    private void Awake()
    {
        gc = GameObject.Find("_GameController").GetComponent<GameController>();
    }

    //Called whenever this block is hit
    public void HitBlock()
    {
        //Destroy block, add score, add speed
        gc.ModifyScore(scoreValue);
        gc.GetBall().ModifySpeed(speedModValue);
        Destroy(gameObject);
    }
}
